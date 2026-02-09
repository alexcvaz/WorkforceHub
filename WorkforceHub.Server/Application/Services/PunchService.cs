using Microsoft.Extensions.Logging;
using WorkforceHub.Server.Application.DTOs;
using WorkforceHub.Server.Application.Interfaces;
using WorkforceHub.Server.Domain.Entities;
using WorkforceHub.Server.Domain.Enums;

namespace WorkforceHub.Server.Application.Services
{
    public class PunchService : IPunchService
    {
        private readonly IPunchRepository _repository;
        private readonly ILogger<PunchService> _logger;

        public PunchService(IPunchRepository repository, ILogger<PunchService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PunchResponse> RegisterPunchAsync(
            string userId,
            CreatePunchRequest request,
            string? ipAddress = null,
            string? userAgent = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Registering punch for user {UserId}", userId);

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var punchType = await DeterminePunchTypeAsync(userId, today, cancellationToken);

            var punch = new Punch(userId, DateTimeOffset.UtcNow, punchType)
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Accuracy = request.Accuracy,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var savedPunch = await _repository.AddAsync(punch, cancellationToken);

            _logger.LogInformation(
                "Punch registered successfully for user {UserId} with type {PunchType}",
                userId,
                punchType);

            return MapToResponse(savedPunch);
        }

        public async Task<List<PunchResponse>> GetTodayPunchesAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var punches = await _repository.GetUserPunchesAsync(userId, today, cancellationToken);

            return punches.Select(MapToResponse).ToList();
        }

        public async Task<List<PunchResponse>> GetPunchesByDateAsync(
            string userId,
            DateOnly date,
            CancellationToken cancellationToken = default)
        {
            var punches = await _repository.GetUserPunchesAsync(userId, date, cancellationToken);

            return punches.Select(MapToResponse).ToList();
        }

        public async Task<List<PunchResponse>> GetPunchesByDateRangeAsync(
            string userId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default)
        {
            var punches = await _repository.GetUserPunchesByDateRangeAsync(
                userId,
                startDate,
                endDate,
                cancellationToken);

            return punches.Select(MapToResponse).ToList();
        }

        private async Task<PunchType> DeterminePunchTypeAsync(
            string userId,
            DateOnly date,
            CancellationToken cancellationToken)
        {
            var lastPunch = await _repository.GetLastUserPunchAsync(userId, date, cancellationToken);

            if (lastPunch == null)
            {
                return PunchType.ClockIn;
            }

            return lastPunch.Type switch
            {
                PunchType.ClockIn => PunchType.LunchStart,
                PunchType.LunchStart => PunchType.LunchEnd,
                PunchType.LunchEnd => PunchType.ClockOut,
                PunchType.ClockOut => PunchType.Extra,
                PunchType.Extra => PunchType.Extra,
                _ => PunchType.ClockIn
            };
        }

        private static PunchResponse MapToResponse(Punch punch)
        {
            return new PunchResponse
            {
                Id = punch.Id,
                Timestamp = punch.Timestamp,
                Type = punch.Type,
                Latitude = punch.Latitude,
                Longitude = punch.Longitude,
                Accuracy = punch.Accuracy,
                IpAddress = punch.IpAddress
            };
        }
    }
}

using WorkforceHub.Server.Application.DTOs;

namespace WorkforceHub.Server.Application.Interfaces
{
    public interface IPunchService
    {
        Task<PunchResponse> RegisterPunchAsync(
            string userId,
            CreatePunchRequest request,
            string? ipAddress = null,
            string? userAgent = null,
            CancellationToken cancellationToken = default);

        Task<List<PunchResponse>> GetTodayPunchesAsync(string userId, CancellationToken cancellationToken = default);

        Task<List<PunchResponse>> GetPunchesByDateAsync(
            string userId,
            DateOnly date,
            CancellationToken cancellationToken = default);

        Task<List<PunchResponse>> GetPunchesByDateRangeAsync(
            string userId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default);
    }
}

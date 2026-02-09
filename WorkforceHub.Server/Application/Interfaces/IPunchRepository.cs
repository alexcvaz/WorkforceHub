using WorkforceHub.Server.Domain.Entities;

namespace WorkforceHub.Server.Application.Interfaces
{
    public interface IPunchRepository
    {
        Task<Punch> AddAsync(Punch punch, CancellationToken cancellationToken = default);

        Task<List<Punch>> GetUserPunchesAsync(string userId, DateOnly date, CancellationToken cancellationToken = default);

        Task<Punch?> GetLastUserPunchAsync(string userId, DateOnly date, CancellationToken cancellationToken = default);

        Task<List<Punch>> GetUserPunchesByDateRangeAsync(
            string userId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default);
    }
}

using Microsoft.EntityFrameworkCore;
using WorkforceHub.Server.Application.Interfaces;
using WorkforceHub.Server.Domain.Entities;
using WorkforceHub.Server.Infrastructure.Data;

namespace WorkforceHub.Server.Infrastructure.Repositories
{
    public class PunchRepository : IPunchRepository
    {
        private readonly WorkforceHubDbContext _context;

        public PunchRepository(WorkforceHubDbContext context)
        {
            _context = context;
        }

        public async Task<Punch> AddAsync(Punch punch, CancellationToken cancellationToken = default)
        {
            _context.Punches.Add(punch);
            await _context.SaveChangesAsync(cancellationToken);
            return punch;
        }

        public async Task<List<Punch>> GetUserPunchesAsync(
            string userId,
            DateOnly date,
            CancellationToken cancellationToken = default)
        {
            // Buscar todos os punches do usuário
            // (SQLite não consegue traduzir DateTimeOffset comparisons)
            var allUserPunches = await _context.Punches
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            // Filtrar em memória por data
            return allUserPunches
                .Where(p => new DateOnly(p.Timestamp.Year, p.Timestamp.Month, p.Timestamp.Day) == date)
                .OrderBy(p => p.Timestamp)
                .ToList();
        }

        public async Task<Punch?> GetLastUserPunchAsync(
            string userId,
            DateOnly date,
            CancellationToken cancellationToken = default)
        {
            // Buscar todos os punches do usuário
            var allUserPunches = await _context.Punches
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            // Filtrar em memória por data
            return allUserPunches
                .Where(p => new DateOnly(p.Timestamp.Year, p.Timestamp.Month, p.Timestamp.Day) == date)
                .OrderByDescending(p => p.Timestamp)
                .FirstOrDefault();
        }

        public async Task<List<Punch>> GetUserPunchesByDateRangeAsync(
            string userId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken = default)
        {
            // Buscar todos os punches do usuário no intervalo
            var allUserPunches = await _context.Punches
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            // Filtrar em memória por data
            return allUserPunches
                .Where(p =>
                {
                    var punchDate = new DateOnly(p.Timestamp.Year, p.Timestamp.Month, p.Timestamp.Day);
                    return punchDate >= startDate && punchDate <= endDate;
                })
                .OrderBy(p => p.Timestamp)
                .ToList();
        }
    }
}

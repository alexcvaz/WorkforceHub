using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkforceHub.Server.Application.Interfaces;
using WorkforceHub.Server.Infrastructure.Data;
using WorkforceHub.Server.Infrastructure.Repositories;

namespace WorkforceHub.Server.Infrastructure.Extensions
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<WorkforceHubDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IPunchRepository, PunchRepository>();

            return services;
        }
    }
}

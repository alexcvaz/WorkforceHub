using Microsoft.Extensions.DependencyInjection;
using WorkforceHub.Server.Application.Interfaces;
using WorkforceHub.Server.Application.Services;

namespace WorkforceHub.Server.Application.Extensions
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPunchService, PunchService>();

            return services;
        }
    }
}

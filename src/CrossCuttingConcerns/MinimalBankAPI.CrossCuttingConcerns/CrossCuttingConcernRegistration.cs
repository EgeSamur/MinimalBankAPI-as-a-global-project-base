using Microsoft.Extensions.DependencyInjection;
using MinimalBankAPI.CrossCuttingConcerns.Exceptions;

namespace MinimalBankAPI.CrossCuttingConcerns
{
    public static class CrossCuttingConcernRegistration
    {
        public static IServiceCollection AddCrossCuttingConcern(this IServiceCollection services)
        {
            services.AddScoped<ExceptionMiddleware>();
            return services;
        }
    }
}

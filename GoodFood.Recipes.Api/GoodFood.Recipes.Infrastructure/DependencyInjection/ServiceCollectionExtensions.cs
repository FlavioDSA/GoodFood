using GoodFood.Recipes.Application.Security.Abstractions;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace GoodFood.Recipes.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
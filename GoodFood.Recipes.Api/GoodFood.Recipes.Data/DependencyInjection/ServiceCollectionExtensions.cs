using AutoMapper;
using GoodFood.Recipes.Application.Common.Paging;
using GoodFood.Recipes.Data.Repositories;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GoodFood.Recipes.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddTransient<IIngredientsRepository, IngredientsRepository>();
            services.AddTransient<IPagedRepository<Ingredient>, IngredientsRepository>();

            services.AddTransient<IRecipesRepository, RecipesRepository>();
            services.AddTransient<IPagedRepository<Recipe>, RecipesRepository>();

            services.AddTransient<IRecipeCategoriesRepository, RecipeCategoriesRepository>();

            services.AddAutoMapper(typeof(RecipesDbContext).Assembly);

            return services;
        }
    }
}
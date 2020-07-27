using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoodFood.Recipes.Data.Repositories
{
    internal sealed class RecipeCategoriesRepository : IRecipeCategoriesRepository
    {
        private readonly IMapper mapper;
        private readonly RecipesDbContext recipesDbContext;

        public RecipeCategoriesRepository(RecipesDbContext recipesDbContext, IMapper mapper)
        {
            this.recipesDbContext = recipesDbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RecipeCategory>> GetAllAsync()
        {
            var categories = await this.recipesDbContext.RecipeCategories
                .ToListAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<RecipeCategory>>(categories);
        }
    }
}
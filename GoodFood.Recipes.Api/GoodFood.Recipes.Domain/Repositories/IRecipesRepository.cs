using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Repositories
{
    public interface IRecipesRepository
    {
        Task<Recipe> AddAsync(Recipe recipe);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Recipe>> GetAsync(string userId);

        Task<Recipe> GetAsync(Guid id);

        Task<Recipe> GetAsync(string userId, string slug);

        Task<bool> UpdateAsync(Recipe recipe);
    }
}
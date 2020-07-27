using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Repositories
{
    public interface IIngredientsRepository
    {
        Task<Ingredient> AddAsync(Ingredient ingredient);

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<Ingredient>> GetAsync(string userId);

        Task<Ingredient> GetAsync(Guid id);

        Task<Ingredient> GetAsync(string userId, string slug);

        Task<bool> UpdateAsync(Ingredient ingredient);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Repositories
{
    public interface IRecipeIngredientsRepository
    {
        Task<IEnumerable<RecipeIngredient>> GetAsync(Guid recipeId);
    }
}
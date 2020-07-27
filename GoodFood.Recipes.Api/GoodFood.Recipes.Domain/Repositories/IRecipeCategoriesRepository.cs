using System.Collections.Generic;
using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Repositories
{
    public interface IRecipeCategoriesRepository
    {
        Task<IEnumerable<RecipeCategory>> GetAllAsync();
    }
}
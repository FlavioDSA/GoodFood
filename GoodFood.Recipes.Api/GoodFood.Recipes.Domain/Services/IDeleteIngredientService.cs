using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Services
{
    public interface IDeleteIngredientService
    {
        Task<bool> ExecuteAsync(Ingredient ingredient);
    }
}
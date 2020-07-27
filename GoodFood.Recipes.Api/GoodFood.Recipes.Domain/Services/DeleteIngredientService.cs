using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Domain.Services
{
    internal class DeleteIngredientService : IDeleteIngredientService
    {
        public Task<bool> ExecuteAsync(Ingredient ingredient)
        {
            // TODO: Create domain logic for deleting an ingredient and avoid leaving this responsibility to the data layer.
            throw new System.NotImplementedException();
        }
    }
}
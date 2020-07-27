using System;

namespace GoodFood.Recipes.Domain.Model
{
    public class RecipeIngredient
    {
        public Guid Id { get; set; }

        public string Amount { get; set; }

        public Guid IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}

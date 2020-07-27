using System;

namespace GoodFood.Recipes.Data.DbModel
{
    public class RecipeIngredientDbModel
    {
        public Guid Id { get; set; }

        public string Amount { get; set; }

        public Guid IngredientId { get; set; }

        public IngredientDbModel Ingredient { get; set; }

        public Guid RecipeId { get; set; }

        public RecipeDbModel Recipe { get; set; }
    }
}

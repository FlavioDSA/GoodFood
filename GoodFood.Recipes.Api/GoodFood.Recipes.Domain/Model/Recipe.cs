using System;
using System.Collections.Generic;

namespace GoodFood.Recipes.Domain.Model
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public IEnumerable<RecipeIngredient> RecipeIngredients { get; set; }

        public Guid RecipeCategoryId { get; set; }

        public RecipeCategory RecipeCategory { get; set; }

        public string AppUserId { get; set; }
    }
}
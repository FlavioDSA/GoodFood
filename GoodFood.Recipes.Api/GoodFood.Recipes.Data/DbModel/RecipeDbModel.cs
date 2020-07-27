using System;
using System.Collections.Generic;
using GoodFood.Recipes.Application.Users.Model;

namespace GoodFood.Recipes.Data.DbModel
{
    public class RecipeDbModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public Guid RecipeCategoryId { get; set; }

        public RecipeCategoryDbModel RecipeCategory { get; set; }

        public List<RecipeIngredientDbModel> RecipeIngredients { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}

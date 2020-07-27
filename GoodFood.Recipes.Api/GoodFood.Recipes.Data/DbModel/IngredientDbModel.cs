using System;
using GoodFood.Recipes.Application.Users.Model;

namespace GoodFood.Recipes.Data.DbModel
{
    public class IngredientDbModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}

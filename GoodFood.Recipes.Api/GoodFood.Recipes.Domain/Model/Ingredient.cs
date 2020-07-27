using System;

namespace GoodFood.Recipes.Domain.Model
{
    public class Ingredient
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string AppUserId { get; set; }
    }
}
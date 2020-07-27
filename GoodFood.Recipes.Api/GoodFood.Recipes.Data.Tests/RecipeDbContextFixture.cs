using System;
using Microsoft.EntityFrameworkCore;

namespace GoodFood.Recipes.Data.Tests
{
    public class RecipeDbContextFixture : IDisposable
    {
        private readonly DbContextOptions<RecipesDbContext> options;

        public RecipeDbContextFixture()
        {
            this.options = new DbContextOptionsBuilder<RecipesDbContext>()
                .UseInMemoryDatabase(databaseName: "RecipesDB")
                .Options;

            this.RecipesDbContext = new RecipesDbContext(this.options);
        }

        public RecipesDbContext RecipesDbContext { get; private set; }

        public void Dispose()
        {
            this.RecipesDbContext.Dispose();
        }
    }
}
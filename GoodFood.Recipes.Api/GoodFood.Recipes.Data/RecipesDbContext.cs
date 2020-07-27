using GoodFood.Recipes.Application.Users.Model;
using GoodFood.Recipes.Data.DbModel;
using GoodFood.Recipes.Data.DbModel.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodFood.Recipes.Data
{
    public class RecipesDbContext : IdentityDbContext<AppUser>
    {
        public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options)
        {
        }

        public DbSet<IngredientDbModel> Ingredients { get; set; }

        public DbSet<RecipeCategoryDbModel> RecipeCategories { get; set; }

        public DbSet<RecipeIngredientDbModel> RecipeIngredients { get; set; }

        public DbSet<RecipeDbModel> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RecipeCategoryDbModelConfiguration());
            builder.ApplyConfiguration(new RecipeDbModelConfiguration());
            builder.ApplyConfiguration(new RecipeIngredientDbModelConfiguration());
            builder.ApplyConfiguration(new IngredientDbModelConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
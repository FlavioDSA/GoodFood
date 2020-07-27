using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodFood.Recipes.Data.DbModel.Configurations
{
    internal class RecipeIngredientDbModelConfiguration : IEntityTypeConfiguration<RecipeIngredientDbModel>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientDbModel> builder)
        {
            builder.Property(x => x.Amount).IsRequired().HasMaxLength(90);

            //builder.HasOne(x => x.Recipe).WithMany(x => x.RecipeIngredients).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Ingredient).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
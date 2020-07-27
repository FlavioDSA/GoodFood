using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodFood.Recipes.Data.DbModel.Configurations
{
    internal class RecipeDbModelConfiguration : IEntityTypeConfiguration<RecipeDbModel>
    {
        public void Configure(EntityTypeBuilder<RecipeDbModel> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(90);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(50);

            builder.HasIndex("AppUserId", "Slug").IsUnique(true);

            builder.Property(x => x.AppUserId).IsRequired();

            builder.HasMany(x => x.RecipeIngredients).WithOne(x => x.Recipe).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
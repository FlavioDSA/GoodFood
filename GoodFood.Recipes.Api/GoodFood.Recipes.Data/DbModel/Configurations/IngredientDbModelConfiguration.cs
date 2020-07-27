using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodFood.Recipes.Data.DbModel.Configurations
{
    internal class IngredientDbModelConfiguration : IEntityTypeConfiguration<IngredientDbModel>
    {
        public void Configure(EntityTypeBuilder<IngredientDbModel> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(90);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(50);

            builder.HasIndex("AppUserId", "Slug").IsUnique(true);

            builder.HasIndex(x => x.Title).IsUnique(false);

            builder.Property(x => x.AppUserId).IsRequired();
        }
    }
}

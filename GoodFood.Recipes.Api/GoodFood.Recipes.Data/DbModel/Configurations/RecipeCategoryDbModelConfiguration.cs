using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodFood.Recipes.Data.DbModel.Configurations
{
    internal class RecipeCategoryDbModelConfiguration : IEntityTypeConfiguration<RecipeCategoryDbModel>
    {
        public void Configure(EntityTypeBuilder<RecipeCategoryDbModel> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(90);
        }
    }
}
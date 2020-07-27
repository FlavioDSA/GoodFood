using AutoMapper;
using GoodFood.Recipes.Application.Users.Model;
using GoodFood.Recipes.Domain.Model;

namespace GoodFood.Recipes.Data.DbModel.ModelMapping
{
    internal sealed class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Ingredient, IngredientDbModel>().ReverseMap();
            CreateMap<RecipeCategory, RecipeCategoryDbModel>().ReverseMap();
            CreateMap<RecipeIngredient, RecipeIngredientDbModel>().ReverseMap();
            CreateMap<Recipe, RecipeDbModel>().ReverseMap();
        }
    }
}
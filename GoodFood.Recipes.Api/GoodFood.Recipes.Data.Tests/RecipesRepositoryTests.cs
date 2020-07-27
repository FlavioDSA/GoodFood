using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using DeepEqual.Syntax;
using FluentAssertions;
using GoodFood.Recipes.Application.Users.Model;
using GoodFood.Recipes.Data.DbModel;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoodFood.Recipes.Data.Tests
{
    public class RecipesRepositoryTests : IClassFixture<RecipeDbContextFixture>
    {
        private readonly Fixture fixture = new Fixture();
        private readonly RecipeDbContextFixture recipeDbContextFixture;
        private readonly IRecipesRepository recipesRepository;
        private readonly IServiceProvider serviceProvider;

        public RecipesRepositoryTests(RecipeDbContextFixture recipeDbContextFixture)
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(RecipesDbContext).Assembly);

            this.serviceProvider = services.BuildServiceProvider();
            this.recipeDbContextFixture = recipeDbContextFixture;

            this.recipesRepository = new Repositories.RecipesRepository(
                this.recipeDbContextFixture.RecipesDbContext,
                this.serviceProvider.GetService<IMapper>()
            );
        }

        [Fact]
        public async Task AddAsync_ValidEntity_InsertsSuccessfully()
        {
            // Arrange

            // Creates user, recipe category and ingredient in DB [Arrange]
            var appUser = this.fixture.Create<AppUser>();

            var recipeCategoryDbModel = this.fixture.Create<RecipeCategoryDbModel>();

            var ingredientDbModel = this.fixture
                .Build<IngredientDbModel>()
                .With(x => x.AppUser, appUser)
                .Create();

            await this.recipeDbContextFixture.RecipesDbContext.Users.AddAsync(appUser).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.RecipeCategories.AddAsync(recipeCategoryDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.Ingredients.AddAsync(ingredientDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            // Builds the recipe [Arrange]
            var recipeIngredients = this.fixture.Build<RecipeIngredient>()
                .With(x => x.IngredientId, ingredientDbModel.Id)
                .Without(x => x.Ingredient)
                .CreateMany(1);

            var recipe = this.fixture
                .Build<Recipe>()
                .With(x => x.AppUserId, appUser.Id)
                .With(x => x.RecipeCategoryId, recipeCategoryDbModel.Id)
                .Without(x => x.RecipeCategory)
                .With(x => x.RecipeIngredients, recipeIngredients)
                .Create();

            // Act
            await this.recipesRepository.AddAsync(recipe).ConfigureAwait(false);

            // Assert
            var recipeDbModel = await this.recipeDbContextFixture.RecipesDbContext.Recipes.AsNoTracking()
                .Include(x => x.RecipeIngredients)
                .FirstOrDefaultAsync(x => x.Id == recipe.Id)
                .ConfigureAwait(false);

            recipeDbModel.Should().NotBeNull();
            recipeDbModel.WithDeepEqual(recipe)
                .IgnoreUnmatchedProperties()
                .Assert();
        }

        [Fact]
        public async Task GetAsync_DbContainsValidEntity_ReturnsValidDomainModel()
        {
            // Arrange
            var appUser = this.fixture.Create<AppUser>();

            var ingredientDbModel = this.fixture
                .Build<IngredientDbModel>()
                .With(x => x.AppUser, appUser)
                .Create();

            var recipeCategoryDbModel = this.fixture.Create<RecipeCategoryDbModel>();

            var recipeIngredientsDbModel = this.fixture.Build<RecipeIngredientDbModel>()
                .With(x => x.Ingredient, ingredientDbModel)
                .Without(x => x.RecipeId)
                .Without(x => x.Recipe)
                .CreateMany(1)
                .ToList();

            var recipeDbModel = this.fixture
                .Build<RecipeDbModel>()
                .With(x => x.AppUser, appUser)
                .With(x => x.RecipeCategory, recipeCategoryDbModel)
                .With(x => x.RecipeIngredients, recipeIngredientsDbModel)
                .Create();

            await this.recipeDbContextFixture.RecipesDbContext.Users.AddAsync(appUser).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.Ingredients.AddAsync(ingredientDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.RecipeCategories.AddAsync(recipeCategoryDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.Recipes.AddAsync(recipeDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var recipe = await this.recipesRepository.GetAsync(recipeDbModel.Id).ConfigureAwait(false);

            // Assert
            recipe.WithDeepEqual(recipeDbModel)
                .IgnoreUnmatchedProperties()
                .Assert();
        }
    }
}
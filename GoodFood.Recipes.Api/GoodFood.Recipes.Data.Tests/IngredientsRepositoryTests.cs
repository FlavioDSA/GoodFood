using System;
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
    public class IngredientsRepositoryTests : IClassFixture<RecipeDbContextFixture>
    {
        private readonly Fixture fixture = new Fixture();
        private readonly IIngredientsRepository ingredientsRepository;
        private readonly RecipeDbContextFixture recipeDbContextFixture;
        private readonly IServiceProvider serviceProvider;

        public IngredientsRepositoryTests(RecipeDbContextFixture recipeDbContextFixture)
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(RecipesDbContext).Assembly);

            this.serviceProvider = services.BuildServiceProvider();
            this.recipeDbContextFixture = recipeDbContextFixture;

            this.ingredientsRepository = new Repositories.IngredientsRepository(
                this.recipeDbContextFixture.RecipesDbContext,
                this.serviceProvider.GetService<IMapper>()
            );
        }

        [Fact]
        public async Task AddAsync_ValidEntity_InsertsSuccessfully()
        {
            // Arrange
            var appUser = this.fixture.Create<AppUser>();
            await this.recipeDbContextFixture.RecipesDbContext.Users.AddAsync(appUser).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            var ingredient = this.fixture
                .Build<Ingredient>()
                .With(x => x.AppUserId, appUser.Id)
                .Create();

            // Act
            await this.ingredientsRepository.AddAsync(ingredient).ConfigureAwait(false);

            // Assert
            var ingredientDbModel = await this.recipeDbContextFixture.RecipesDbContext.Ingredients
                .FirstOrDefaultAsync(x => x.Id == ingredient.Id).ConfigureAwait(false);

            ingredientDbModel.Should().NotBeNull();
            ingredientDbModel.WithDeepEqual(ingredient).IgnoreSourceProperty(x => x.AppUser).Assert();
        }

        [Fact]
        public async Task GetAsync_DbContainsValidEntity_ReturnsValidDomainModel()
        {
            // Arrange
            var appUser = this.fixture.Create<AppUser>();

            var ingredientDbModel = this.fixture
                .Build<IngredientDbModel>()
                .With(x => x.AppUserId, appUser.Id)
                .Create();

            await this.recipeDbContextFixture.RecipesDbContext.Users.AddAsync(appUser).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.Ingredients.AddAsync(ingredientDbModel).ConfigureAwait(false);
            await this.recipeDbContextFixture.RecipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            // Act
            var ingredient = await this.ingredientsRepository.GetAsync(ingredientDbModel.Id).ConfigureAwait(false);

            // Assert
            ingredient.WithDeepEqual(ingredientDbModel).IgnoreDestinationProperty(x => x.AppUser).Assert();
        }
    }
}
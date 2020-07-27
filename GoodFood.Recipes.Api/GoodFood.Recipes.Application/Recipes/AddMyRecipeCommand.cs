using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GoodFood.Recipes.Application.Common.Exceptions;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class AddMyRecipeCommand
    {
        public class NewRecipe : IRequest<Recipe>
        {
            public string Description { get; set; }

            public string Slug { get; set; }

            public string Title { get; set; }

            public Guid RecipeCategoryId { get; set; }

            public IEnumerable<NewRecipeIngredient> RecipeIngredients { get; set; }
        }

        public class NewRecipeIngredient
        {
            public string Amount { get; set; }

            public Guid IngredientId { get; set; }
        }

        public class NewRecipeValidator : AbstractValidator<NewRecipe>
        {
            public NewRecipeValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Title).NotEmpty().MaximumLength(90);
                RuleFor(x => x.RecipeIngredients).NotEmpty();
            }
        }

        public class NewRecipeIngredientValidator : AbstractValidator<NewRecipeIngredient>
        {
            public NewRecipeIngredientValidator()
            {
                RuleFor(x => x.Amount).NotEmpty().MaximumLength(90);
                RuleFor(x => x.IngredientId).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<NewRecipe, Recipe>
        {
            private readonly IRecipesRepository recipesRepository;
            private readonly IUserContext userContext;

            public Handler(IRecipesRepository recipesRepository, IUserContext userContext)
            {
                this.recipesRepository = recipesRepository;
                this.userContext = userContext;
            }

            public async Task<Recipe> Handle(NewRecipe request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var recipe = new Recipe
                {
                    Description = request.Description,
                    Slug = request.Slug,
                    Title = request.Title,
                    AppUserId = appUser.Id,
                    RecipeCategoryId = request.RecipeCategoryId,
                    RecipeIngredients = request.RecipeIngredients.Select(x =>
                        new RecipeIngredient
                        {
                            IngredientId = x.IngredientId,
                            Amount = x.Amount
                        })
                };

                recipe = await this.recipesRepository.AddAsync(recipe).ConfigureAwait(false);

                if (recipe == null)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Error creating recipe");
                }

                return recipe;
            }
        }
    }
}
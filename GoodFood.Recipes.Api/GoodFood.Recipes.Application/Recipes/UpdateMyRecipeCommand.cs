using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class UpdateMyRecipeCommand
    {
        public class UpdateCommand : IRequest<bool>
        {
            public UpdateCommand(Guid id, UpdatedRecipe updatedRecipe)
            {
                Id = id;
                UpdatedRecipe = updatedRecipe;
            }

            public Guid Id { get; set; }

            public UpdatedRecipe UpdatedRecipe { get; set; }
        }

        public class UpdatedRecipe
        {
            public string Description { get; set; }

            public string Title { get; set; }

            public Guid RecipeCategoryId { get; set; }

            public IEnumerable<UpdatedRecipeIngredient> RecipeIngredients { get; set; }
        }

        public class UpdatedRecipeIngredient 
        {
            public string Amount { get; set; }

            public Guid IngredientId { get; set; }
        }

        public class UpdatedRecipeValidator : AbstractValidator<UpdatedRecipe>
        {
            public UpdatedRecipeValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Title).NotEmpty().MaximumLength(90);
                RuleFor(x => x.RecipeCategoryId).NotEmpty();
                RuleFor(x => x.RecipeIngredients).NotEmpty();
            }
        }

        public class UpdatedRecipeIngredientValidator : AbstractValidator<UpdatedRecipeIngredient>
        {
            public UpdatedRecipeIngredientValidator()
            {
                RuleFor(x => x.Amount).NotEmpty().MaximumLength(90);
                RuleFor(x => x.IngredientId).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<UpdateCommand, bool>
        {
            private readonly IRecipesRepository recipesRepository;
            private readonly IUserContext userContext;

            public Handler(IRecipesRepository recipesRepository, IUserContext userContext)
            {
                this.recipesRepository = recipesRepository;
                this.userContext = userContext;
            }

            public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var recipe = new Recipe
                {
                    Id = request.Id,
                    Description = request.UpdatedRecipe.Description,
                    Title = request.UpdatedRecipe.Title,
                    RecipeCategoryId = request.UpdatedRecipe.RecipeCategoryId,
                    RecipeIngredients = request.UpdatedRecipe.RecipeIngredients.Select(x =>
                        new RecipeIngredient 
                        {
                            Amount = x.Amount,
                            IngredientId = x.IngredientId
                        }).ToList(),
                    AppUserId = appUser.Id
                };

                return await this.recipesRepository.UpdateAsync(recipe).ConfigureAwait(false);
            }
        }
    }
}

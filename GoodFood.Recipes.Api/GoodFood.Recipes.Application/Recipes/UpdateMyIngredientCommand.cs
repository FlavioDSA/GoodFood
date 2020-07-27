using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class UpdateMyIngredientCommand
    {
        public class UpdateCommand : IRequest<bool>
        {
            public UpdateCommand(Guid id, UpdatedIngredient updatedIngredient)
            {
                Id = id;
                UpdatedIngredient = updatedIngredient;
            }

            public Guid Id { get; set; }

            public UpdatedIngredient UpdatedIngredient { get; set; }
        }

        public class UpdatedIngredient
        {
            public string Description { get; set; }
            public string Title { get; set; }
        }

        public class UpdatedIngredientValidator : AbstractValidator<UpdatedIngredient>
        {
            public UpdatedIngredientValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Title).NotEmpty().MaximumLength(90);
            }
        }

        internal sealed class Handler : IRequestHandler<UpdateCommand, bool>
        {
            private readonly IIngredientsRepository ingredientsRepository;
            private readonly IUserContext userContext;

            public Handler(IIngredientsRepository ingredientsRepository, IUserContext userContext)
            {
                this.ingredientsRepository = ingredientsRepository;
                this.userContext = userContext;
            }

            public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var ingredient = new Ingredient
                {
                    Id = request.Id,
                    Description = request.UpdatedIngredient.Description,
                    Title = request.UpdatedIngredient.Title,
                    AppUserId = appUser.Id
                };

                return await this.ingredientsRepository.UpdateAsync(ingredient).ConfigureAwait(false);
            }
        }
    }
}
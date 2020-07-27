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
    public class AddMyIngredientCommand
    {
        public class NewIngredient : IRequest<Ingredient>
        {
            public string Description { get; set; }

            public string Slug { get; set; }

            public string Title { get; set; }
        }

        public class NewIngredientValidator : AbstractValidator<NewIngredient>
        {
            public NewIngredientValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Slug).NotEmpty().MaximumLength(90);
                RuleFor(x => x.Title).NotEmpty().MaximumLength(90);
            }
        }

        internal sealed class Handler : IRequestHandler<NewIngredient, Ingredient>
        {
            private readonly IIngredientsRepository ingredientsRepository;
            private readonly IUserContext userContext;

            public Handler(IIngredientsRepository ingredientsRepository, IUserContext userContext)
            {
                this.ingredientsRepository = ingredientsRepository;
                this.userContext = userContext;
            }

            public async Task<Ingredient> Handle(NewIngredient request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var ingredient = new Ingredient
                {
                    Description = request.Description,
                    Slug = request.Slug,
                    Title = request.Title,
                    AppUserId = appUser.Id
                };

                ingredient = await this.ingredientsRepository.AddAsync(ingredient).ConfigureAwait(false);

                if (ingredient == null)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Error creating ingredient");
                }

                return ingredient;
            }
        }
    }
}
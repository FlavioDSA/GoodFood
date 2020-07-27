using System;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyIngredientByIdQuery
    {
        public class Query : IRequest<Ingredient>
        {
            public Query(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, Ingredient>
        {
            private readonly IIngredientsRepository ingredientsRepository;
            private readonly IUserContext userContext;

            public Handler(IIngredientsRepository ingredientsRepository, IUserContext userContext)
            {
                this.ingredientsRepository = ingredientsRepository;
                this.userContext = userContext;
            }

            public async Task<Ingredient> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                if (appUser == null)
                {
                    return null;
                }

                var ingredient = await this.ingredientsRepository.GetAsync(request.Id).ConfigureAwait(false);

                if (ingredient == null || ingredient.AppUserId != appUser.Id)
                {
                    return null;
                }

                return ingredient;
            }
        }
    }
}
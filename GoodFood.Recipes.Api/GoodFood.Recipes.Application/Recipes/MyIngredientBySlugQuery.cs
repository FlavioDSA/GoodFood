using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyIngredientBySlugQuery
    {
        public class Query : IRequest<Ingredient>
        {
            public Query(string slug)
            {
                Slug = slug;
            }

            public string Slug { get; }
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

                return await this.ingredientsRepository.GetAsync(appUser.Id, request.Slug).ConfigureAwait(false);
            }
        }
    }
}
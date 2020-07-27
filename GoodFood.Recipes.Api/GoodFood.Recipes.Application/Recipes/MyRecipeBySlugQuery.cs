using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyRecipeBySlugQuery
    {
        public class Query : IRequest<Recipe>
        {
            public Query(string slug)
            {
                Slug = slug;
            }

            public string Slug { get; }
        }

        internal sealed class Handler : IRequestHandler<Query, Recipe>
        {
            private readonly IRecipesRepository recipesRepository;
            private readonly IUserContext userContext;

            public Handler(IRecipesRepository recipesRepository, IUserContext userContext)
            {
                this.recipesRepository = recipesRepository;
                this.userContext = userContext;
            }

            public async Task<Recipe> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                if (appUser == null)
                {
                    return null;
                }

                return await this.recipesRepository.GetAsync(appUser.Id, request.Slug).ConfigureAwait(false);
            }
        }
    }
}
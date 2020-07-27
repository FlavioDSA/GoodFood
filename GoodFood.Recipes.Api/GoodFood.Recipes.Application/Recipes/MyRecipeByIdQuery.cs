using System;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyRecipeByIdQuery
    {
        public class Query : IRequest<Recipe>
        {
            public Query(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
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

                var recipe = await this.recipesRepository.GetAsync(request.Id).ConfigureAwait(false);

                if (recipe == null || recipe.AppUserId != appUser.Id)
                {
                    return null;
                }

                return recipe;
            }
        }
    }
}

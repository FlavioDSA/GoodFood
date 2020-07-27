using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Common.Paging;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyRecipesQuery
    {
        public class Query : IRequest<PagedResult<Recipe>>
        {
            public Query(PagedRequest pagedRequest)
            {
                if (pagedRequest == null)
                {
                    pagedRequest = PagedRequest.Default;
                }

                PagedRequest = pagedRequest;
            }

            public PagedRequest PagedRequest { get; }
        }

        internal sealed class Handler : IRequestHandler<Query, PagedResult<Recipe>>
        {
            private readonly IPagedRepository<Recipe> recipesRepository;
            private readonly IUserContext userContext;

            public Handler(IPagedRepository<Recipe> recipesRepository, IUserContext userContext)
            {
                this.recipesRepository = recipesRepository;
                this.userContext = userContext;
            }

            public async Task<PagedResult<Recipe>> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                if (appUser == null)
                {
                    return new PagedResult<Recipe>(request.PagedRequest, 0, Enumerable.Empty<Recipe>());
                }

                return await this.recipesRepository.GetAsync(appUser.Id, request.PagedRequest).ConfigureAwait(false);
            }
        }
    }
}
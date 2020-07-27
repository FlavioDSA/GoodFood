using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Common.Paging;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Model;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class MyIngredientsQuery
    {
        public class Query : IRequest<PagedResult<Ingredient>>
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

        internal sealed class Handler : IRequestHandler<Query, PagedResult<Ingredient>>
        {
            private readonly IPagedRepository<Ingredient> ingredientsRepository;
            private readonly IUserContext userContext;

            public Handler(IPagedRepository<Ingredient> ingredientsRepository, IUserContext userContext)
            {
                this.ingredientsRepository = ingredientsRepository;
                this.userContext = userContext;
            }

            public async Task<PagedResult<Ingredient>> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                if (appUser == null)
                {
                    return new PagedResult<Ingredient>(request.PagedRequest, 0, Enumerable.Empty<Ingredient>());
                }

                return await this.ingredientsRepository.GetAsync(appUser.Id, request.PagedRequest).ConfigureAwait(false);
            }
        }
    }
}
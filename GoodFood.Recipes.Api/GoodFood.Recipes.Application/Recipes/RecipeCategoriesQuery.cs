using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class RecipeCategoriesQuery
    {
        public class GetAllQuery : IRequest<IEnumerable<RecipeCategory>>
        {
        }

        internal sealed class Handler : IRequestHandler<GetAllQuery, IEnumerable<RecipeCategory>>
        {
            private readonly IRecipeCategoriesRepository recipeCategoriesRepository;

            public Handler(IRecipeCategoriesRepository recipeCategoriesRepository)
            {
                this.recipeCategoriesRepository = recipeCategoriesRepository;
            }

            public async Task<IEnumerable<RecipeCategory>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                return await this.recipeCategoriesRepository.GetAllAsync().ConfigureAwait(false);
            }
        }
    }
}
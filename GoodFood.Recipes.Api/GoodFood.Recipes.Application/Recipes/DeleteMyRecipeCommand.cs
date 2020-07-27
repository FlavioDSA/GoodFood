using System;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class DeleteMyRecipeCommand
    {
        public class Command : IRequest<bool>
        {
            public Command(Guid id)
            {
                Id = id;
            }
            public Guid Id { get; }
        }

        internal sealed class Handler : IRequestHandler<Command, bool>
        {
            private readonly IRecipesRepository recipesRepository;
            private readonly IUserContext userContext;

            public Handler(IRecipesRepository recipesRepository, IUserContext userContext)
            {
                this.recipesRepository = recipesRepository;
                this.userContext = userContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var recipe = await this.recipesRepository.GetAsync(request.Id).ConfigureAwait(false);

                if (recipe == null)
                {
                    return false;
                }

                if (recipe.AppUserId != appUser.Id)
                {
                    return false;
                }

                return await this.recipesRepository.DeleteAsync(recipe.Id);
            }
        }
    }
}
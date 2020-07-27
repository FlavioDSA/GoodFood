using System;
using System.Threading;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Domain.Repositories;
using MediatR;

namespace GoodFood.Recipes.Application.Recipes
{
    public class DeleteMyIngredientCommand
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
            private readonly IIngredientsRepository ingredientsRepository;
            private readonly IUserContext userContext;

            public Handler(IIngredientsRepository ingredientsRepository, IUserContext userContext)
            {
                this.ingredientsRepository = ingredientsRepository;
                this.userContext = userContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = await this.userContext.GetUserAsync().ConfigureAwait(false);

                var ingredient = await this.ingredientsRepository.GetAsync(request.Id).ConfigureAwait(false);

                if (ingredient == null)
                {
                    return false;
                }

                if (ingredient.AppUserId != appUser.Id)
                {
                    return false;
                }

                return await this.ingredientsRepository.DeleteAsync(ingredient.Id);
            }
        }
    }
}
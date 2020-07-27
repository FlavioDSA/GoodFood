using GoodFood.Recipes.Application.Users.Model;
using MediatR;

namespace GoodFood.Recipes.Application.Users
{
    // TODO create user registration command
    public class Register
    {
        public class UserProfile : IRequest<User>
        {
        }
    }
}
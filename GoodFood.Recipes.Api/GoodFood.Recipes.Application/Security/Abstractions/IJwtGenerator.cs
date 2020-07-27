using GoodFood.Recipes.Application.Users.Model;

namespace GoodFood.Recipes.Application.Security.Abstractions
{
    public interface IJwtGenerator
    {
        string CreateToken(AppUser user);
    }
}
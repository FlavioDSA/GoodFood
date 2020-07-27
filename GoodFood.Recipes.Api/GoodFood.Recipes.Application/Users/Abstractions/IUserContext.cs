using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Model;

namespace GoodFood.Recipes.Application.Users.Abstractions
{
    public interface IUserContext
    {
        Task<AppUser> GetUserAsync();
    }
}
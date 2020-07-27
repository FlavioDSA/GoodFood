using Microsoft.AspNetCore.Identity;

namespace GoodFood.Recipes.Application.Users.Model
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
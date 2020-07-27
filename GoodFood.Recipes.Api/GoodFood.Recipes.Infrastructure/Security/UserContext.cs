using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Abstractions;
using GoodFood.Recipes.Application.Users.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GoodFood.Recipes.Infrastructure.Security
{
    internal sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<AppUser> userManager;
        private AppUser appUser;

        public UserContext(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<AppUser> GetUserAsync()
        {
            if (this.appUser != null)
            {
                return this.appUser;
            }

            var username = this.httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            this.appUser = await this.userManager.FindByNameAsync(username).ConfigureAwait(false);

            return this.appUser;
        }
    }
}
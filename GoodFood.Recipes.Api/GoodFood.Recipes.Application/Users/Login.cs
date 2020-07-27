using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using GoodFood.Recipes.Application.Common.Exceptions;
using GoodFood.Recipes.Application.Security.Abstractions;
using GoodFood.Recipes.Application.Users.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodFood.Recipes.Application.Users
{
    public class Login
    {
        public class Credentials : IRequest<User>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class CredentialsValidator : AbstractValidator<Credentials>
        {
            public CredentialsValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Credentials, User>
        {
            private readonly IJwtGenerator jwtGenerator;
            private readonly SignInManager<AppUser> signInManager;
            private readonly UserManager<AppUser> userManager;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Credentials request, CancellationToken cancellationToken)
            {
                var appUser = await this.userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                if (appUser == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var signInResult = await this.signInManager.CheckPasswordSignInAsync(appUser, request.Password, false).ConfigureAwait(false);

                if (!signInResult.Succeeded)
                    throw new RestException(HttpStatusCode.Unauthorized);

                return new User
                {
                    DisplayName = appUser.DisplayName,
                    Token = this.jwtGenerator.CreateToken(appUser),
                    UserName = appUser.UserName
                };
            }
        }
    }
}
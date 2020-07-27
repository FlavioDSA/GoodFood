using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodFood.Recipes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] Login.Credentials credentials)
        {
            var user = await this.mediator.Send(credentials).ConfigureAwait(false);

            return Ok(user);
        }
    }
}
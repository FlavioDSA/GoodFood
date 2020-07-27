using System.Linq;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Recipes;
using GoodFood.Recipes.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodFood.Recipes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeCategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public RecipeCategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<RecipeCategory>> GetAllAsync()
        {
            var categories = await this.mediator.Send(new RecipeCategoriesQuery.GetAllQuery()).ConfigureAwait(false);

            return Ok(categories ?? Enumerable.Empty<RecipeCategory>());
        }
    }
}
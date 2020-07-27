using System;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Common.Paging;
using GoodFood.Recipes.Application.Recipes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodFood.Recipes.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MyRecipesController : ControllerBase
    {
        private readonly IMediator mediator;

        public MyRecipesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddMyRecipeCommand.NewRecipe newRecipe)
        {
            var recipe = await this.mediator.Send(newRecipe).ConfigureAwait(false);

            return CreatedAtRoute(routeName: "GetRecipeBySlug", routeValues: new { recipe.Slug }, value: recipe);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagedRequest pagedRequest)
        {
            var pagedResult = await this.mediator.Send(new MyRecipesQuery.Query(pagedRequest)).ConfigureAwait(false);

            return Ok(pagedResult);
        }

        //[HttpGet("{id}", Name = "GetRecipeById")]
        //public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        //{
        //    var recipe = await this.mediator.Send(new MyRecipeByIdQuery.Query(id)).ConfigureAwait(false);

        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(recipe);
        //}

        [HttpGet("{slug}", Name = "GetRecipeBySlug")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string slug)
        {
            var recipe = await this.mediator.Send(new MyRecipeBySlugQuery.Query(slug)).ConfigureAwait(false);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateMyRecipeCommand.UpdatedRecipe updatedRecipe)
        {
            var updatedSuccessfully = await this.mediator.Send(
                new UpdateMyRecipeCommand.UpdateCommand(id, updatedRecipe)).ConfigureAwait(false);

            if (!updatedSuccessfully)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id)
        {
            var deletedSuccessfully = await this.mediator.Send(new DeleteMyRecipeCommand.Command(id)).ConfigureAwait(false);

            if (!deletedSuccessfully)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
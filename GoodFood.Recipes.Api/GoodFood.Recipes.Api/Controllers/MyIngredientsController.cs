using System;
using System.Net;
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
    public class MyIngredientsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MyIngredientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddMyIngredientCommand.NewIngredient newIngredient)
        {
            var ingredient = await this.mediator.Send(newIngredient).ConfigureAwait(false);

            return CreatedAtRoute(routeName: "GetIngredientBySlug", routeValues: new { ingredient.Slug }, value: ingredient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id)
        {
            bool deletedSuccessfully = false;

            try
            {
                deletedSuccessfully = await this.mediator.Send(new DeleteMyIngredientCommand.Command(id)).ConfigureAwait(false);
            }
            catch (InvalidOperationException ex)
            {
                // TODO: Avoid throwing Exception and create domain logic to handle this.
                // TODO: Remove this responsibility from the data layer.
                return new StatusCodeResult((int)HttpStatusCode.PreconditionFailed);
            }

            if (!deletedSuccessfully)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PagedRequest pagedRequest)
        {
            var pagedResult = await this.mediator.Send(new MyIngredientsQuery.Query(pagedRequest)).ConfigureAwait(false);

            return Ok(pagedResult);
        }

        [HttpGet("{slug}", Name = "GetIngredientBySlug")]
        public async Task<IActionResult> GetBySlugAsync([FromRoute] string slug)
        {
            var ingredient = await this.mediator.Send(new MyIngredientBySlugQuery.Query(slug)).ConfigureAwait(false);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateMyIngredientCommand.UpdatedIngredient updatedIngredient)
        {
            var updatedSuccessfully = await this.mediator.Send(
                new UpdateMyIngredientCommand.UpdateCommand(id, updatedIngredient)).ConfigureAwait(false);

            if (!updatedSuccessfully)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
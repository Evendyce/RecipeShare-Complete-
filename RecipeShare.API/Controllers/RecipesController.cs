using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeShare.API.Services;
using RecipeShare.Models.Shared;
using System;

namespace RecipeShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _service;

        public RecipesController(IRecipeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRecipes(
            [FromQuery] RecipeSearchDto? filter,
            [FromQuery] bool useTile = false)
        {
            if (useTile)
            {
                var tiles = await _service.GetAllTilesAsync(filter);
                return Ok(tiles);
            }

            var recipes = await _service.GetAllRecipesAsync(filter);
            return Ok(recipes);
        }

        [HttpGet("recipes-for-user")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecipesForUser([FromQuery] RecipeSearchDto? filter)
        {
            var tiles = await _service.GetAllTilesForUserAsync(filter);

            return Ok(tiles);
        }
        
        [HttpGet("my-recipes")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyRecipes([FromQuery] RecipeSearchDto? filter)
        {
            var tiles = await _service.GetMyRecipesTilesAsync(filter);

            return Ok(tiles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(long id, [FromQuery] string? username)
        {
            var dto = await _service.GetByIdAsync(id, username);

            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Optional: redundant if [ApiController] is used

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetRecipe), new { id = created.Id }, created); // <-- returns 201 Created
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(long id, [FromBody] RecipeDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);

            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(long id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }


        [HttpPost("favourite-recipe")]
        public async Task<IActionResult> ToggleFavourite([FromBody] FavouriteToggleRequestDto request)
        {
            var result = await _service.ToggleFavouriteAsync(request);

            return result ? Ok() : BadRequest("Could not toggle favourite.");
        }
    }
}

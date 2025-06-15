using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeShare.API.Data;
using RecipeShare.API.Services;
using RecipeShare.Models.Shared;

namespace RecipeShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TagDto>>> GetAllTagNames()
        {
            var tagNames = await _tagService.GetAllTagsAsync();

            return Ok(tagNames);
        }

        // Placeholder for future CRUD
        [HttpPost]
        public IActionResult CreateTag([FromBody] TagDto tag)
        {
            return StatusCode(501, "Tag creation not implemented yet.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] TagDto tag)
        {
            return StatusCode(501, "Tag update not implemented yet.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            return StatusCode(501, "Tag deletion not implemented yet.");
        }
    }
}

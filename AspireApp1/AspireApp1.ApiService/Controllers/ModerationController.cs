using AspireApp1.ApiService.Application.Moderation;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp1.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModerationController : ControllerBase
    {
        private readonly IModerationService _moderationService;

        public ModerationController(IModerationService moderationService)
        {
            _moderationService = moderationService;
        }

        [HttpPost("/posts/{id}/tag")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Moderator")]
        public async Task<IActionResult> TagPost(int id, [FromBody] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return BadRequest("Tag cannot be empty.");

            await _moderationService.TagPostAsync(id, tag);
            return Ok(new { PostId = id, Tag = tag });
        }
    }
}

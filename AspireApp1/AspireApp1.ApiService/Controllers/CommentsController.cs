using Microsoft.AspNetCore.Mvc;
using AspireApp1.ApiService.Domain.Interfaces;
using AspireApp1.ApiService.Application.Comments;

namespace AspireApp1.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpPost("/posts/{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Content cannot be empty.");

            var userId = Application.Auth.AuthUtils.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            var comment = await _commentsService.AddCommentAsync(id, userId.Value, content);
            return Ok(comment);
        }

        [HttpGet("/posts/{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _commentsService.GetCommentsByPostIdAsync(id);
            return Ok(comments);
        }
    }
}

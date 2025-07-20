using Microsoft.AspNetCore.Mvc;
using AspireApp1.ApiService.Application.Posts;
using AspireApp1.ApiService.Application.Auth;

namespace AspireApp1.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] GetPostsQuery query)
        {
            var posts = await _postService.GetPostsAsync(query);
            return Ok(posts);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var userId = AspireApp1.ApiService.Application.Auth.AuthUtils.GetUserIdFromClaims(User);
            if (userId == null) return Unauthorized();
            await _postService.CreatePostAsync(command, userId.Value);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost("{id}/like")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> LikePost(int id)
        {
            var userId = AuthUtils.GetUserIdFromClaims(User);
            if (userId == null) return Unauthorized();
            try
            {
                await _postService.LikePostAsync(id, userId.Value);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/like")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> UnlikePost(int id)
        {
            var userId = AuthUtils.GetUserIdFromClaims(User);
            if (userId == null) return Unauthorized();
            await _postService.UnlikePostAsync(id, userId.Value);
            return Ok();
        }

    }
}

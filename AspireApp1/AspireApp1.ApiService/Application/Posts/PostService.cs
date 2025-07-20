using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspireApp1.ApiService.Application.Posts
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPostsAsync(GetPostsQuery query);
        Task<Post> GetPostByIdAsync(int id);
        Task CreatePostAsync(CreatePostCommand command, int userId);
        Task LikePostAsync(int id, int userId);
        Task UnlikePostAsync(int id, int userId);
    }

    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(GetPostsQuery query)
        {
            return await _postRepository.FilterAsync(query.FromDate, query.ToDate, query.UserId, query.Tags, query.SortBy);
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task CreatePostAsync(CreatePostCommand command, int userId)
        {
            var post = new Post
            {
                Title = command.Title,
                Content = command.Content,
                UserId = userId,
                CreatedAt = System.DateTime.UtcNow,
                Likes = 0
            };
            await _postRepository.AddAsync(post);
        }

        public async Task LikePostAsync(int id, int userId)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException("Post not found.");

            if (post.UserId == userId)
                throw new System.InvalidOperationException("Users cannot like their own posts.");

            // Check if the user already liked the post
            var comments = await _postRepository.GetAllAsync(); // This is just a placeholder; you should have a method to check likes
            // Ideally, add a method in IPostRepository: Task<bool> HasUserLikedPostAsync(int postId, int userId)
            if (await _postRepository.HasUserLikedPostAsync(id, userId))
                throw new System.InvalidOperationException("User has already liked this post.");

            await _postRepository.LikeAsync(id, userId);
        }

        public async Task UnlikePostAsync(int id, int userId)
        {
            await _postRepository.UnlikeAsync(id, userId);
        }
    }
}

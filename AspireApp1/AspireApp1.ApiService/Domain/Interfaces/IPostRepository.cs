using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Domain.Interfaces;
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task AddAsync(Post post);
        #nullable enable
        Task<IEnumerable<Post>> FilterAsync(System.DateTime? fromDate, System.DateTime? toDate, int? userId, List<string>? tags, string? sortBy);
        Task LikeAsync(int postId, int userId);
        Task UnlikeAsync(int postId, int userId);
        Task AddTagToPostAsync(int postId, int tagId);
        Task<bool> HasUserLikedPostAsync(int postId, int userId);
    }

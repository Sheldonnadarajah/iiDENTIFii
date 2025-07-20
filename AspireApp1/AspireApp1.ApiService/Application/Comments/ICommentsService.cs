using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Application.Comments
{
    public interface ICommentsService
    {
        Task<Comment> AddCommentAsync(int postId, int userId, string content);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    }
}

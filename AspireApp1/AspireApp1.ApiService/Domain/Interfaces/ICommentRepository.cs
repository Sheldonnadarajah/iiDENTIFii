using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
        Task AddAsync(Comment comment);
    }
}

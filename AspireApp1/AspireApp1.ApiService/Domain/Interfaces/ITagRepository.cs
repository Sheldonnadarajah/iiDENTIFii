using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetByIdAsync(int id);
        Task<Tag> GetByNameAsync(string name);
        Task AddAsync(Tag tag);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<int?> GetTagIdByNameAsync(string tag);
        Task<int> AddTagAsync(string tag);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;

namespace AspireApp1.ApiService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
    }
}

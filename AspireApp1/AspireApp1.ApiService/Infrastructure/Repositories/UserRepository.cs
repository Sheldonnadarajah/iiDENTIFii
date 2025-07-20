
using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AspireApp1.ApiService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _dbConnection;

        public UserRepository(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username", new { Username = username });
        }

        public async Task AddAsync(User user)
        {
            // Ensure Role is set
            if (user.Role == 0)
                user.Role = Domain.Entities.UserRole.Regular;
            await _dbConnection.ExecuteAsync(
                "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES (@Username, @Email, @PasswordHash, @Role)", user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<User>("SELECT * FROM Users");
        }
    }
}

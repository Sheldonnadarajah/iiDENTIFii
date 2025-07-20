
using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AspireApp1.ApiService.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly SqlConnection _dbConnection;

        public CommentRepository(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<Comment>(
                "SELECT * FROM Comments WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
        {
            return await _dbConnection.QueryAsync<Comment>(
                "SELECT * FROM Comments WHERE PostId = @PostId", new { PostId = postId });
        }

        public async Task AddAsync(Comment comment)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO Comments (PostId, UserId, Content, CreatedAt) VALUES (@PostId, @UserId, @Content, @CreatedAt)", comment);
        }
    }
}

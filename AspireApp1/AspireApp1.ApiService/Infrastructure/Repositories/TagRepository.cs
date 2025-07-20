
using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AspireApp1.ApiService.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
    private readonly SqlConnection _dbConnection;

    public TagRepository(SqlConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<int?> GetTagIdByNameAsync(string tag)
    {
        var sql = "SELECT Id FROM Tags WHERE Name = @Name";
        return await _dbConnection.QueryFirstOrDefaultAsync<int?>(sql, new { Name = tag });
    }

    public async Task<int> AddTagAsync(string tag)
    {
        var sql = "INSERT INTO Tags (Name) OUTPUT INSERTED.Id VALUES (@Name)";
        return await _dbConnection.ExecuteScalarAsync<int>(sql, new { Name = tag });
    }

    public async Task<Tag> GetByIdAsync(int id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Tag>(
            "SELECT * FROM Tags WHERE Id = @Id", new { Id = id });
    }

    public async Task<Tag> GetByNameAsync(string name)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Tag>(
            "SELECT * FROM Tags WHERE Name = @Name", new { Name = name });
    }

    public async Task AddAsync(Tag tag)
    {
        await _dbConnection.ExecuteAsync(
            "INSERT INTO Tags (Name) VALUES (@Name)", tag);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _dbConnection.QueryAsync<Tag>("SELECT * FROM Tags");
    }
    }
}


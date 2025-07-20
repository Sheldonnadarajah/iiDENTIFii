
using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AspireApp1.ApiService.Infrastructure.Repositories
       
{
    public class PostRepository : IPostRepository
    {
        private readonly SqlConnection _dbConnection;

        public PostRepository(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddTagToPostAsync(int postId, int tagId)
        {
            var sql = "INSERT INTO PostTags (PostId, TagId) VALUES (@PostId, @TagId)";
            await _dbConnection.ExecuteAsync(sql, new { PostId = postId, TagId = tagId });
        }
        public async Task<Post> GetByIdAsync(int id)
        {
            var sql = @"SELECT p.*, COUNT(l.Id) AS Likes
                        FROM Posts p
                        LEFT JOIN Likes l ON l.PostId = p.Id
                        WHERE p.Id = @Id
                        GROUP BY p.Id, p.Title, p.Content, p.UserId, p.CreatedAt";
            return await _dbConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
        }


        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            var sql = @"SELECT p.*, COUNT(l.Id) AS Likes
                        FROM Posts p
                        LEFT JOIN Likes l ON l.PostId = p.Id
                        GROUP BY p.Id, p.Title, p.Content, p.UserId, p.CreatedAt";
            return await _dbConnection.QueryAsync<Post>(sql);
        }

        public async Task AddAsync(Post post)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO Posts (Title, Content, UserId, CreatedAt) VALUES (@Title, @Content, @UserId, @CreatedAt)", post);
        }
        #nullable enable annotations
        public async Task<IEnumerable<Post>> FilterAsync(DateTime? fromDate, DateTime? toDate, int? userId, List<string>? tags, string? sortBy)
        {
            var sqlPosts = @"SELECT p.Id, p.Title, p.Content, p.UserId, p.CreatedAt, COUNT(l.Id) AS Likes
                             FROM Posts p
                             LEFT JOIN Likes l ON l.PostId = p.Id
                             WHERE 1=1";
            var sqlTags = @"SELECT pt.PostId, t.Name AS TagName
                            FROM PostTags pt
                            INNER JOIN Tags t ON t.Id = pt.TagId
                            WHERE 1=1";
            var parameters = new DynamicParameters();

            // Filtering logic for posts
            if (userId.HasValue)
            {
                sqlPosts += " AND p.UserId = @UserId";
                parameters.Add("UserId", userId.Value);
            }

            if (fromDate.HasValue && toDate.HasValue)
            {
                sqlPosts += " AND p.CreatedAt BETWEEN @StartDate AND @EndDate";
                parameters.Add("StartDate", fromDate.Value);
                parameters.Add("EndDate", toDate.Value);
            }
            else if (fromDate.HasValue)
            {
                sqlPosts += " AND p.CreatedAt >= @StartDate";
                parameters.Add("StartDate", fromDate.Value);
            }
            else if (toDate.HasValue)
            {
                sqlPosts += " AND p.CreatedAt <= @EndDate";
                parameters.Add("EndDate", toDate.Value);
            }

            sqlPosts += " GROUP BY p.Id, p.Title, p.Content, p.UserId, p.CreatedAt";

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.ToLower() == "date")
                    sqlPosts += " ORDER BY p.CreatedAt DESC";
                else if (sortBy.ToLower() == "likes")
                    sqlPosts += " ORDER BY Likes DESC";
            }

            // Filtering logic for tags (if tag filter is provided)
            if (tags != null && tags.Count > 0)
            {
                sqlTags += " AND t.Name IN @Tags";
                parameters.Add("Tags", tags);
            }

            // Compose the final SQL for QueryMultiple
            var sql = sqlPosts + "; " + sqlTags + ";";

            using var multi = await _dbConnection.QueryMultipleAsync(sql, parameters);

            var posts = (await multi.ReadAsync<Post>()).ToList();
            var tagTuples = (await multi.ReadAsync<(int PostId, string TagName)>()).ToList();

            // Map tags to posts
            var postDict = posts.ToDictionary(p => p.Id);
            foreach (var (postId, tagName) in tagTuples)
            {
                if (postDict.TryGetValue(postId, out var post))
                {
                    post.Tags ??= new List<string>();
                    post.Tags.Add(tagName);
                }
            }

            return posts;
        }

        public async Task LikeAsync(int postId, int userId)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO Likes (PostId, UserId) VALUES (@PostId, @UserId)",
                new { PostId = postId, UserId = userId });
        }

        public async Task UnlikeAsync(int postId, int userId)
        {
            await _dbConnection.ExecuteAsync(
                "DELETE FROM Likes WHERE PostId = @PostId AND UserId = @UserId",
                new { PostId = postId, UserId = userId });
        }
        
        public async Task<bool> HasUserLikedPostAsync(int postId, int userId)
        {
            var sql = "IF EXISTS (SELECT 1 FROM Likes WHERE PostId = @PostId AND UserId = @UserId) SELECT CAST(1 AS BIT) ELSE SELECT CAST(0 AS BIT)";
            return await _dbConnection.ExecuteScalarAsync<bool>(sql, new { PostId = postId, UserId = userId });
        }
    }
}

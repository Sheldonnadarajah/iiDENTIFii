using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;

namespace AspireApp1.ApiService.Application.Comments
{
public class CommentsService : ICommentsService
{
        private readonly ICommentRepository _commentRepository;

        public CommentsService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Comment> AddCommentAsync(int postId, int userId, string content)
        {
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            await _commentRepository.AddAsync(comment);
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _commentRepository.GetByPostIdAsync(postId);
        }
    }
}

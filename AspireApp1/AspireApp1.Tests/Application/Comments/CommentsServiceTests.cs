using System.Threading.Tasks;
using AspireApp1.ApiService.Application.Comments;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AspireApp1.Tests.Application.Comments
{
    [TestFixture]
    public class CommentsServiceTests
    {
        private Mock<ICommentRepository> _commentRepositoryMock;
        private ICommentsService _commentsService;

        [SetUp]
        public void SetUp()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentsService = new CommentsService(_commentRepositoryMock.Object);
        }

        [Test]
        public async Task AddCommentAsync_ShouldReturnCommentWithCorrectProperties()
        {
            // Arrange
            int postId = 1;
            int userId = 2;
            string content = "Test comment";
            Comment? addedComment = null;
            _commentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Comment>()))
                .Callback<Comment>(c => addedComment = c)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _commentsService.AddCommentAsync(postId, userId, content);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                result.PostId.Should().Be(postId);
                result.UserId.Should().Be(userId);
                result.Content.Should().Be(content);
                result.CreatedAt.Should().BeCloseTo(System.DateTime.UtcNow, System.TimeSpan.FromSeconds(5));
                addedComment.Should().BeEquivalentTo(result);
            }
        }

        [Test]
        public async Task GetCommentsByPostIdAsync_ShouldReturnCommentsForGivenPostId()
        {
            // Arrange
            int postId = 1;
            var comments = new List<Comment>
            {
                new Comment { PostId = postId, UserId = 2, Content = "Comment 1" },
                new Comment { PostId = postId, UserId = 3, Content = "Comment 2" }
            };
            _commentRepositoryMock.Setup(r => r.GetByPostIdAsync(postId)).ReturnsAsync(comments);

            // Act
            var result = await _commentsService.GetCommentsByPostIdAsync(postId);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().HaveCount(2);
                result.All(c => c.PostId == postId).Should().BeTrue();
            }
        }

        [Test]
        public async Task GetCommentsByPostIdAsync_ShouldReturnEmpty_WhenNoCommentsExist()
        {
            // Arrange
            int postId = 99;
            _commentRepositoryMock.Setup(r => r.GetByPostIdAsync(postId)).ReturnsAsync(new List<Comment>());

            // Act
            var result = await _commentsService.GetCommentsByPostIdAsync(postId);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeEmpty();
            }
        }
    }
}

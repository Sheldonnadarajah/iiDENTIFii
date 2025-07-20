using System.Collections.Generic;
using System.Threading.Tasks;
using AspireApp1.ApiService.Application.Posts;
using AspireApp1.ApiService.Domain.Entities;
using AspireApp1.ApiService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AspireApp1.Tests.Application.Posts
{
    [TestFixture]
    public class PostServiceTests
    {
        private Mock<IPostRepository> _postRepositoryMock;
        private IPostService _postService;

        [SetUp]
        public void SetUp()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _postService = new PostService(_postRepositoryMock.Object);
        }

        [Test]
        public async Task GetPostsAsync_ShouldReturnFilteredPosts()
        {
            // Arrange
            var query = new GetPostsQuery { UserId = 1, FromDate = null, ToDate = null };
            var posts = new List<Post> { new Post { Id = 1, UserId = 1, Title = "A" } };
            _postRepositoryMock.Setup(r => r.FilterAsync(query.FromDate, query.ToDate, query.UserId, query.Tags, query.SortBy)).ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsAsync(query);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                result.Should().BeEquivalentTo(posts);
            }
        }

        [Test]
        public async Task GetPostByIdAsync_ShouldReturnPost()
        {
            // Arrange
            var post = new Post { Id = 1, Title = "Test" };
            _postRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

            // Act
            var result = await _postService.GetPostByIdAsync(1);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                result.Should().Be(post);
            }
        }

        [Test]
        public async Task LikePostAsync_ShouldCallRepository_WhenUserHasNotLiked()
        {
            // Arrange
            int postId = 1;
            int userId = 2;
            _postRepositoryMock.Setup(r => r.HasUserLikedPostAsync(postId, userId)).ReturnsAsync(false);
            _postRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(new Post { Id = postId, UserId = 3 });
            _postRepositoryMock.Setup(r => r.LikeAsync(postId, userId)).Returns(Task.CompletedTask);

            // Act
            await _postService.LikePostAsync(postId, userId);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                _postRepositoryMock.Verify(r => r.LikeAsync(postId, userId), Times.Once);
            }
        }

        [Test]
        public void LikePostAsync_ShouldThrow_WhenUserLikesOwnPost()
        {
            // Arrange
            int postId = 1;
            int userId = 2;
            _postRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(new Post { Id = postId, UserId = userId });

            // Act
            var act = async () => await _postService.LikePostAsync(postId, userId);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                act.Should().ThrowAsync<System.InvalidOperationException>().WithMessage("Users cannot like their own posts.");
            }
        }

        [Test]
        public void LikePostAsync_ShouldThrow_WhenUserAlreadyLiked()
        {
            // Arrange
            int postId = 1;
            int userId = 2;
            _postRepositoryMock.Setup(r => r.GetByIdAsync(postId)).ReturnsAsync(new Post { Id = postId, UserId = 3 });
            _postRepositoryMock.Setup(r => r.HasUserLikedPostAsync(postId, userId)).ReturnsAsync(true);

            // Act
            var act = async () => await _postService.LikePostAsync(postId, userId);

            // Assert
            using (new FluentAssertions.Execution.AssertionScope())
            {
                act.Should().ThrowAsync<System.InvalidOperationException>().WithMessage("User has already liked this post.");
            }
        }
    }
}

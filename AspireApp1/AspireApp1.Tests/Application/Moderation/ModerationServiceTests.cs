using System.Threading.Tasks;
using AspireApp1.ApiService.Application.Moderation;
using AspireApp1.ApiService.Domain.Interfaces;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Execution;

namespace AspireApp1.Tests.Application.Moderation
{
    [TestFixture]
    public class ModerationServiceTests
    {
        private Mock<ITagRepository> _tagRepositoryMock;
        private Mock<IPostRepository> _postRepositoryMock;
        private IModerationService _moderationService;

        [SetUp]
        public void SetUp()
        {
            _tagRepositoryMock = new Mock<ITagRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _moderationService = new ModerationService(_tagRepositoryMock.Object, _postRepositoryMock.Object);
        }

        [Test]
        public async Task TagPostAsync_ShouldAddTagToPost_WhenTagExists()
        {
            _tagRepositoryMock.Setup(r => r.GetTagIdByNameAsync("Tech")).ReturnsAsync(1);
            _postRepositoryMock.Setup(r => r.AddTagToPostAsync(2, 1)).Returns(Task.CompletedTask);
            await _moderationService.TagPostAsync(2, "Tech");
            using (new AssertionScope())
            {
                _postRepositoryMock.Verify(r => r.AddTagToPostAsync(2, 1), Times.Once);
            }
        }

        [Test]
        public async Task TagPostAsync_ShouldAddTagToPost_WhenTagDoesNotExist()
        {
            _tagRepositoryMock.Setup(r => r.GetTagIdByNameAsync("NewTag")).ReturnsAsync((int?)null);
            _tagRepositoryMock.Setup(r => r.AddTagAsync("NewTag")).ReturnsAsync(5);
            _postRepositoryMock.Setup(r => r.AddTagToPostAsync(3, 5)).Returns(Task.CompletedTask);
            await _moderationService.TagPostAsync(3, "NewTag");
            using (new AssertionScope())
            {
                _postRepositoryMock.Verify(r => r.AddTagToPostAsync(3, 5), Times.Once);
            }
        }
    }
}

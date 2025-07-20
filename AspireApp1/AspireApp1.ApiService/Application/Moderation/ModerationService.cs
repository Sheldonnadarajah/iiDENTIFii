using System.Threading.Tasks;
using AspireApp1.ApiService.Domain.Interfaces;
using AspireApp1.ApiService.Application.Moderation;

namespace AspireApp1.ApiService.Application.Moderation
{
public class ModerationService : IModerationService
{
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;

        public ModerationService(ITagRepository tagRepository, IPostRepository postRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
        }

        public async Task TagPostAsync(int postId, string tag)
        {
            var tagId = await _tagRepository.GetTagIdByNameAsync(tag)
                        ?? await _tagRepository.AddTagAsync(tag);

            await _postRepository.AddTagToPostAsync(postId, tagId);
        }
    }
}

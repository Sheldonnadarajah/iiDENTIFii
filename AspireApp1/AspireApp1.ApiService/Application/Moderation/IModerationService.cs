using System.Threading.Tasks;

namespace AspireApp1.ApiService.Application.Moderation
{
    public interface IModerationService
    {
        Task TagPostAsync(int postId, string tag);
    }
}

using Podcast.API.Models;

namespace Podcast.API.Services
{
    public interface IPodcastService
    {
        Task<IEnumerable<Podcasts>> GetAllPodcastsAsync();
        Task<Podcasts> GetPodcastByIdAsync(int id);
        Task<Podcasts> CreatePodcastAsync(Podcasts podcast);
        Task UpdatePodcastAsync(Podcasts podcast);
        Task DeletePodcastAsync(int id);
    }
}

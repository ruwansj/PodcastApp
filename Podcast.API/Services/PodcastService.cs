using Microsoft.EntityFrameworkCore;
using Podcast.API.Data;
using Podcast.API.Models;

namespace Podcast.API.Services
{
    public class PodcastService : IPodcastService
    {
        private readonly PodcastDbContext _context;
        private readonly ILogger<PodcastService> _logger;

        public PodcastService(PodcastDbContext context, ILogger<PodcastService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Podcasts>> GetAllPodcastsAsync()
        {
            return await _context.Podcasts.ToListAsync();
        }

        public async Task<Podcasts> GetPodcastByIdAsync(int id)
        {
            return await _context.Podcasts.FindAsync(id);
        }

        public async Task<Podcasts> CreatePodcastAsync(Podcasts podcast)
        {
            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();
            return podcast;
        }

        public async Task UpdatePodcastAsync(Podcasts podcast)
        {
            _context.Entry(podcast).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePodcastAsync(int id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast != null)
            {
                _context.Podcasts.Remove(podcast);
                await _context.SaveChangesAsync();
            }
        }
    }
}

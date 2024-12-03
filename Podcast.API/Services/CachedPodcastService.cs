using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using Podcast.API.Models;

namespace Podcast.API.Services
{
    public class PodcastCacheDecorator : IPodcastService
    {
        private readonly IPodcastService _podcastService;
        private readonly IDistributedCache _cache;
        private readonly ILogger<PodcastCacheDecorator> _logger;

        public PodcastCacheDecorator(
            IPodcastService podcastService,
            IDistributedCache cache,
            ILogger<PodcastCacheDecorator> logger)
        {
            _podcastService = podcastService;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Podcasts>> GetAllPodcastsAsync()
        {
            string cacheKey = "all-podcasts";

            var podcasts = await GetFromCache<IEnumerable<Podcasts>>(cacheKey);
            if (podcasts != null)
            {
                _logger.LogInformation("Cache hit for all podcasts");
                return podcasts;
            }

            podcasts = await _podcastService.GetAllPodcastsAsync();
            await SetCache(cacheKey, podcasts);
            return podcasts;
        }

        public async Task<Podcasts> GetPodcastByIdAsync(int id)
        {
            string cacheKey = $"podcast-{id}";

            var podcast = await GetFromCache<Podcasts>(cacheKey);
            if (podcast != null)
            {
                _logger.LogInformation("Cache hit for podcast {Id}", id);
                return podcast;
            }

            podcast = await _podcastService.GetPodcastByIdAsync(id);
            if (podcast != null)
            {
                await SetCache(cacheKey, podcast);
                _logger.LogInformation("Cache miss for podcast {Id}, added to cache", id);
            }

            return podcast;
        }

        public async Task<Podcasts> CreatePodcastAsync(Podcasts podcast)
        {
            var result = await _podcastService.CreatePodcastAsync(podcast);
            await InvalidateCache("all-podcasts");
            return result;
        }

        public async Task UpdatePodcastAsync(Podcasts podcast)
        {
            await _podcastService.UpdatePodcastAsync(podcast);
            await InvalidateCache($"podcast-{podcast.Id}");
            await InvalidateCache("all-podcasts");
        }

        public async Task DeletePodcastAsync(int id)
        {
            await _podcastService.DeletePodcastAsync(id);
            await InvalidateCache($"podcast-{id}");
            await InvalidateCache("all-podcasts");
        }

        private async Task<T> GetFromCache<T>(string key) where T : class
        {
            var cachedBytes = await _cache.GetAsync(key);
            if (cachedBytes == null) return null;

            var cachedString = Encoding.UTF8.GetString(cachedBytes);
            return JsonSerializer.Deserialize<T>(cachedString);
        }

        private async Task SetCache<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            };

            var jsonString = JsonSerializer.Serialize(value);
            var bytes = Encoding.UTF8.GetBytes(jsonString);
            await _cache.SetAsync(key, bytes, options);
        }

        private async Task InvalidateCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}

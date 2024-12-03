using Microsoft.AspNetCore.Mvc;
using Podcast.API.Models;
using Podcast.API.Services;


namespace Podcast.API.Controllers
{

[ApiController]
    [Route("api/[controller]")]
    public class PodcastsController : ControllerBase
    {
        private readonly ILogger<PodcastsController> _logger;
        private readonly IPodcastService _podcastService;

        public PodcastsController(ILogger<PodcastsController> logger, IPodcastService podcastService)
        {
            _logger = logger;
            _podcastService = podcastService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcasts>>> GetPodcasts()
        {
            var podcasts = await _podcastService.GetAllPodcastsAsync();
            return Ok(podcasts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Podcasts>> GetPodcast(int id)
        {
            var podcast = await _podcastService.GetPodcastByIdAsync(id);
            if (podcast == null)
                return NotFound();

            return Ok(podcast);
        }

        [HttpPost]
        public async Task<ActionResult<Podcasts>> CreatePodcast(Podcasts podcast)
        {
            var created = await _podcastService.CreatePodcastAsync(podcast);
            return CreatedAtAction(nameof(GetPodcast), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePodcast(int id, Podcasts podcast)
        {
            if (id != podcast.Id)
                return BadRequest();

            await _podcastService.UpdatePodcastAsync(podcast);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePodcast(int id)
        {
            await _podcastService.DeletePodcastAsync(id);
            return NoContent();
        }
   
    }
}

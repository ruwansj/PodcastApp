using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;

namespace Podcast.API.Data
{
    public class PodcastDbContext : DbContext
    {
        public PodcastDbContext(DbContextOptions<PodcastDbContext> options) : base(options) { }

        public DbSet<Podcasts> Podcasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Podcasts>()
                .HasIndex(p => p.Title);
        }
    }
}

using Podcast.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Podcast.API.Services.Podcast.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PodcastService>();
builder.Services.AddScoped<IPodcastService>(serviceProvider =>
{
    var podcastService = serviceProvider.GetRequiredService<PodcastService>();
    var cache = serviceProvider.GetRequiredService<IDistributedCache>();
    var logger = serviceProvider.GetRequiredService<ILogger<PodcastCacheDecorator>>();

    return new PodcastCacheDecorator(podcastService, cache, logger);
});
builder.Services.AddScoped<IAudioFileService, AudioFileService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "PodcastApp_";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

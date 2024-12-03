using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Podcast.API.Services
{
    namespace Podcast.API.Services
    {
        public interface IAudioFileService
        {
            Task<string> UploadAudioFileAsync(IFormFile file);
        }

        public class AudioFileService : IAudioFileService
        {
            private readonly IConfiguration _configuration;
            private readonly BlobServiceClient _blobServiceClient;
            private readonly string _containerName;
            private readonly ILogger<AudioFileService> _logger;

            public AudioFileService(IConfiguration configuration, ILogger<AudioFileService> logger)
            {
                _configuration = configuration;
                _logger = logger;
                _blobServiceClient = new BlobServiceClient(_configuration["Azure:StorageConnectionString"]);
                _containerName = _configuration["Azure:BlobContainerName"];
            }

            public async Task<string> UploadAudioFileAsync(IFormFile file)
            {
                try
                {
                    // Validate file
                    if (file == null || file.Length == 0)
                        throw new BadRequestException("No file was uploaded.");

                    if (!IsAudioFile(file))
                        throw new BadRequestException("File must be an audio file.");

                    // Create container if it doesn't exist
                    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                    await containerClient.CreateIfNotExistsAsync();
                    await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

                    // Generate unique filename
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var blobClient = containerClient.GetBlobClient(fileName);

                    // Upload file
                    using var stream = file.OpenReadStream();
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    });

                    _logger.LogInformation($"File {fileName} uploaded successfully");
                    return blobClient.Uri.ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading file");
                    throw;
                }
            }

            private bool IsAudioFile(IFormFile file)
            {
                var allowedTypes = new[]
                {
                "audio/mpeg",    // MP3
                "audio/wav",     // WAV
                "audio/ogg",     // OGG
                "audio/aac",     // AAC
                "audio/m4a"      // M4A
            };
                return allowedTypes.Contains(file.ContentType.ToLower());
            }
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }
    }
}

using ImageService.Models;
using System.IO;

namespace ImageService.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ILogger<ImageRepository> _logger;
        private readonly IConfiguration _configuration;

        public ImageRepository (ILogger<ImageRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public ImageDTO? GetImage(int imageId)
        {
            try
            {
                string uploadDirectory = _configuration["ImagePath"];

                string regexPattern = $"^{imageId}\\..*"; // Matches any file starting with imageId
                var regex = new System.Text.RegularExpressions.Regex(regexPattern);

                // Get all files in the directory that match the pattern
                string? matchingFilePath = Directory.GetFiles(uploadDirectory)
                                                    .FirstOrDefault(filePath => regex.IsMatch(Path.GetFileName(filePath)));

                if (matchingFilePath != null)
                {
                    ImageDTO imageDTO = new ImageDTO { ImageId = imageId, Image = matchingFilePath };
                    return imageDTO;
                }
                
                _logger.LogInformation($"Image with ID {imageId} not found.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving image: {ex.Message}");
                return null;
            }
        }

        public ImageDTO? UploadImage(FormFile image)
        {
            if (image == null || image.Length == 0)
            {
                _logger.LogError("No valid image file provided.");
                return null;
            }

            try
            {
                string uploadDirectory = Environment.GetEnvironmentVariable("ImagePath");

                int imageId = GenerateUniqueId();
                string fileExtension = Path.GetExtension(image.FileName);

                string imagePath = Path.Combine(uploadDirectory, $"{imageId}{fileExtension}");

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                var imageDTO = new ImageDTO
                {
                    ImageId = imageId,
                    Image = imagePath,
                };

                return imageDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading image: {ex.Message}");
                return null;
            }
        }

        public void DeleteImage(int imageId)
        {
            ImageDTO image = GetImage(imageId);

            if (image != null)
            {
                try
                {
                    File.Delete(image.Image);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error deleting image", ex.Message);
                    throw;
                }
            }

            throw new DirectoryNotFoundException();
        }

        private static int GenerateUniqueId()
        {
            int id = Math.Abs(Guid.NewGuid().GetHashCode());

            return id;
        }
    }
}

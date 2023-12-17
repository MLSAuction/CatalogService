using Microsoft.AspNetCore.Mvc;
using ImageService.Repositories;
using ImageService.Models;
using Microsoft.AspNetCore.Authorization;

namespace ImageService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageRepository _repository;

        public ImageController(ILogger<ImageController> logger, IConfiguration configuration, IImageRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// Gets an image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {

            ImageDTO image = _repository.GetImage(id);

            if (image == null)
            {
                return NotFound(); // Return 404 if image is not found
            }

            _logger.LogInformation($"Image {image.ImageId} Retrieved ");

            return Ok(image);
        }

        /// <summary>
        /// Uploads an image from a file and returns the object with id and name
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadImage(IFormFile file)
        {
            FormFile imageFile = (FormFile)Request.Form.Files[0];

            if (imageFile == null)
            {
                return BadRequest();
            }

            ImageDTO? imageDTO = _repository.UploadImage(imageFile);

            if (imageDTO != null)
            {
                return Ok(imageDTO);
            }

            return BadRequest();
        }

        /// <summary>
        /// Deletes an image by id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public IActionResult Delete(int imageId) 
        {
            try
            {
                _repository.DeleteImage(imageId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
            return Ok();
        }
    }
}

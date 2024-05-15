using MicroBlog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(ImageService imageService) : Controller
    {
        private readonly ImageService _imageService = imageService;

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var imageUrl = await _imageService.SaveImageAsync(file);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}

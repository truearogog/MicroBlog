using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController(IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Return the URL of the uploaded image
                var imageUrl = "/uploads/" + uniqueFileName;
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}

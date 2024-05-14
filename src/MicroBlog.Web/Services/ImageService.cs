using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MicroBlog.Web.Services
{
    public class ImageService(IWebHostEnvironment webHostEnvironment)
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        private async Task<string> SaveImageAsyncInternal(IFormFile file, Func<Stream, string, Task> saveAction)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var path = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, path);

            using var stream = file.OpenReadStream();
            await saveAction(stream, filePath);
            
            return "/uploads/" + path;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            return await SaveImageAsyncInternal(file, async (stream, path) =>
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                await stream.CopyToAsync(fileStream);
            });
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            return await SaveImageAsyncInternal(file, async (stream, path) =>
            {
                using var image = Image.Load(stream);
                var size = Math.Min(image.Width, image.Height);
                image.Mutate(x => x.Crop(new Rectangle((image.Width - size) / 2, (image.Height - size) / 2, size, size)).Resize(200, 200));
                await image.SaveAsync(path);
            });
        }

        public void Delete(string url)
        {
            var fileName = url.Split('/').Last();
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            File.Delete(filePath);
        }
    }
}

using MicroBlog.Core.Repositories;
using MicroBlog.Identity.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MicroBlog.Web.Services
{
    public class ImageService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, 
        IImageRepository imageRepository)
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IImageRepository _imageRepository = imageRepository;

        private async Task<string> SaveImageAsyncInternal(IFormFile file, Func<Stream, string, Task> saveAction)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var path = Guid.NewGuid().ToString() + "_" + file.FileName;
            var absolutePath = Path.Combine(uploadsFolder, path);

            using var stream = file.OpenReadStream();
            await saveAction(stream, absolutePath);

            var filePath = "/uploads/" + path;
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId()!;
            await _imageRepository.Create(new Core.Models.Image { Path = filePath, UserId = userId });

            return filePath;
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

        public async Task Delete(string url)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId()!;
            await _imageRepository.Delete(new Core.Models.Image { Path = url, UserId = userId });

            var filePath = _webHostEnvironment.WebRootPath + url;
            File.Delete(filePath);
        }

        public async Task DeleteForUser(string userId)
        {
            var images = _imageRepository.GetAll(x => x.UserId == userId).Select(x => x.Path).ToList();
            foreach (var image in images)
            {
                await Delete(image);
            }
        }
    }
}
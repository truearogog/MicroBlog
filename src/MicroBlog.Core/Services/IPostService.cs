using MicroBlog.Core.Models;

namespace MicroBlog.Core.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPostsFromUserAsync(string userId, int skip, int take);
        Task<IEnumerable<Post>> GetPostsForUserAsync(string userId, int skip, int take);
    }
}

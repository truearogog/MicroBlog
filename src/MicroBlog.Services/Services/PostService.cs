using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;

namespace MicroBlog.Services.Services
{
    public class PostService(IPostRepository postRepository, UserManager userManager) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly UserManager _userManager = userManager;

        public async Task<IEnumerable<Post>> GetPostsFromUserAsync(string userId, int skip, int take)
        {
            var posts = _postRepository.GetAll(x => x.UserId == userId).Skip(skip).Take(take).ToList();
            foreach (var post in posts)
            {
                post.UserProfilePictureUrl = await _userManager.GetProfilePictureUrlAsync(userId);
                post.UserName = await _userManager.GetUserNameAsync(userId);
            }
            return posts;
        }

        public Task<IEnumerable<Post>> GetPostsForUserAsync(string userId, int skip, int take)
        {
            throw new NotImplementedException();
        }
    }
}

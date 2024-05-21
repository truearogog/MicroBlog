using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController(IPostService postService) : Controller
    {
        private readonly IPostService _postService = postService;

        [HttpGet("FromUser")]
        public async Task<IActionResult> FromUser(string userId, int skip, int take)
        {
            var posts = await _postService.GetPostsFromUserAsync(userId, skip, take);
            return PartialView("_PostList", posts);
        }

        [HttpGet("ForUser")]
        public async Task<IActionResult> ForUser(string userId, int skip, int take)
        {
            var posts = await _postService.GetPostsForUserAsync(userId, skip, take);
            return PartialView("_PostList", posts);
        }

        [HttpGet("ForTags")]
        public async Task<IActionResult> ForTags(string userId, int skip, int take)
        {
            throw new NotImplementedException();
        }
    }
}

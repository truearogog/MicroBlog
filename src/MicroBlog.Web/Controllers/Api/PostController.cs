using MicroBlog.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService) : Controller
    {
        private readonly IPostService _postService = postService;

        [HttpGet("FromUser")]
        public async Task<IActionResult> FromUser(string userId, int skip, int take)
        {
            var posts = await _postService.GetPostsFromUserAsync(userId, skip, take);
            return PartialView(@"_PostList", posts);
        }
    }
}

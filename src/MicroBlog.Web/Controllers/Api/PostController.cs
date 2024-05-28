using MicroBlog.Core.Constants;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController(IPostService postService, IPostRepository postRepository, IReactionRepository reactionRepository) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IReactionRepository _reactionRepository = reactionRepository;

        [HttpGet("FromUser")]
        public async Task<IActionResult> FromUser(string userId, string before, int skip, int take)
        {
            if (!DateTime.TryParse(before, out var beforeDateTime))
            {
                return BadRequest();
            }
            var posts = await _postService.GetPostsFromUserAsync(userId, beforeDateTime, skip, take);
            return PartialView("~/Pages/Shared/Post/_PostList.cshtml", posts);
        }

        [HttpGet("ForUser")]
        public async Task<IActionResult> ForUser(string userId, string before, int skip, int take)
        {
            if (!DateTime.TryParse(before, out var beforeDateTime))
            {
                return BadRequest();
            }
            var posts = await _postService.GetPostsForUserAsync(userId, beforeDateTime, skip, take);
            return PartialView("~/Pages/Shared/Post/_PostList.cshtml", posts);
        }

        [HttpPost("CreateReaction")]
        public async Task<IActionResult> CreateReaction([FromForm] Guid postId, [FromForm] ReactionType type)
        {
            if (!await _postRepository.Any(x => x.Id == postId))
            {
                return NotFound($"No post with id {{'{postId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _reactionRepository.Create(new Core.Models.Reaction { PostId = postId, UserId = currentUserId, Type = type });

            return Ok();
        }

        [HttpPost("DeleteReaction")]
        public async Task<IActionResult> DeleteReaction([FromForm] Guid postId, [FromForm] ReactionType type)
        {
            if (!await _postRepository.Any(x => x.Id == postId))
            {
                return NotFound($"No post with id {{'{postId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _reactionRepository.Delete(new Core.Models.Reaction { PostId = postId, UserId = currentUserId, Type = type });

            return Ok();
        }

        [HttpPost("DeletePost")]
        public async Task<IActionResult> DeletePost([FromForm] Guid postId)
        {
            try
            {
                var userId = User.GetUserId()!;
                if (!await _postRepository.Any(x => x.Id == postId && x.UserId == userId))
                {
                    return Unauthorized();
                }

                await _postRepository.Delete(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

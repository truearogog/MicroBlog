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
    public class PostController(IPostService postService, IPostRepository postRepository, IReactionRepository reactionRepository, 
        ICommentService commentService) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IReactionRepository _reactionRepository = reactionRepository;
        private readonly ICommentService _commentService = commentService;

        [HttpGet("FromUser")]
        public async Task<IActionResult> FromUser(string userId, DateTime before, int skip, int take)
        {
            var posts = await _postService.GetPostsFromUserAsync(userId, before, skip, take);
            return PartialView("~/Pages/Shared/Post/_PostList.cshtml", posts);
        }

        [HttpGet("ForUser")]
        public async Task<IActionResult> ForUser(string userId, DateTime before, int skip, int take)
        {
            var posts = await _postService.GetPostsForUserAsync(userId, before, skip, take);
            return PartialView("~/Pages/Shared/Post/_PostList.cshtml", posts);
        }

        [HttpGet("Comments")]
        public async Task<IActionResult> Comments(Guid postId, DateTime before, int skip, int take)
        {
            var comments = await _commentService.GetComments(postId, before, skip, take);
            return PartialView("~/Pages/Shared/Comment/_CommentList.cshtml", comments);
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromForm] Guid postId, [FromForm] string content)
        {
            var userId = User.GetUserId()!;
            var comment = await _commentService.CreateComment(new Core.Models.Comment { PostId = postId, UserId = userId, Content = content });
            return PartialView("~/Pages/Shared/Comment/_Comment.cshtml", comment);
        }

        [HttpPost("AddReaction")]
        public async Task<IActionResult> AddReaction([FromForm] Guid postId, [FromForm] ReactionType type)
        {
            if (!await _postRepository.Any(x => x.Id == postId))
            {
                return NotFound($"No post with id {{'{postId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _reactionRepository.Create(new Core.Models.Reaction { PostId = postId, UserId = currentUserId, Type = type });

            return Ok();
        }

        [HttpPost("RemoveReaction")]
        public async Task<IActionResult> RemoveReaction([FromForm] Guid postId, [FromForm] ReactionType type)
        {

            if (!await _postRepository.Any(x => x.Id == postId))
            {
                return NotFound($"No post with id {{'{postId}'}}.");
            }

            var currentUserId = User.GetUserId()!;
            await _reactionRepository.Delete(new Core.Models.Reaction { PostId = postId, UserId = currentUserId, Type = type });

            return Ok();
        }
    }
}

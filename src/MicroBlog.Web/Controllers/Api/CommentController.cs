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
    public class CommentController(ICommentService commentService, ICommentRepository commentRepository) : Controller
    {
        private readonly ICommentService _commentService = commentService;
        private readonly ICommentRepository _commentRepository = commentRepository;

        [HttpGet("Comments")]
        public async Task<IActionResult> Comments(Guid postId, string before, int skip, int take)
        {
            if (!DateTime.TryParse(before, out var beforeDateTime))
            {
                return BadRequest();
            }
            var comments = await _commentService.GetComments(postId, beforeDateTime, skip, take);
            return PartialView("~/Pages/Shared/Comment/_CommentList.cshtml", comments);
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment([FromForm] Guid postId, [FromForm] string content)
        {
            var userId = User.GetUserId()!;
            var comment = await _commentService.CreateComment(new Core.Models.Comment { PostId = postId, UserId = userId, Content = content });
            return PartialView("~/Pages/Shared/Comment/_Comment.cshtml", comment);
        }

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromForm] Guid commentId)
        {
            try
            {
                var userId = User.GetUserId()!;
                if (!await _commentRepository.Any(x => x.Id == commentId && x.UserId == userId))
                {
                    return Unauthorized();
                }

                await _commentRepository.Delete(commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

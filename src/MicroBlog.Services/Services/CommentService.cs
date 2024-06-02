using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;

namespace MicroBlog.Services.Services
{
    public class CommentService(ICommentRepository commentRepository, IUserManager userManager) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IUserManager _userManager = userManager;

        private async Task LoadComment(Comment comment)
        {
            comment.UserProfilePictureUrl = await _userManager.GetProfilePictureUrlAsync(comment.UserId);
            comment.UserName = await _userManager.GetUserNameAsync(comment.UserId);
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid postId, DateTime before, int skip, int take)
        {
            var comments = _commentRepository.GetAll(x => x.PostId == postId && x.Created < before).Skip(skip).Take(take).ToList();
            foreach (var comment in comments)
            {
                await LoadComment(comment);
            }
            return comments;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            var newComment = await _commentRepository.Create(comment);
            await LoadComment(newComment);
            return newComment;
        }
    }
}

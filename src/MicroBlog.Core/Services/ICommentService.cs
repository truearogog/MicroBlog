using MicroBlog.Core.Models;

namespace MicroBlog.Core.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetComments(Guid postId, DateTime before, int skip, int take);
        Task<Comment> CreateComment(Comment comment);
    }
}

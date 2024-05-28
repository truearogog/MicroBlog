using MicroBlog.Core.Models;

namespace MicroBlog.Core.Repositories
{
    public interface ICommentRepository : IUserRelatedRepository<Comment>
    {
        Task Delete(Guid id);
    }
}

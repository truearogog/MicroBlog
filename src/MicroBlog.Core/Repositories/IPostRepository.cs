using MicroBlog.Core.Models;

namespace MicroBlog.Core.Repositories
{
    public interface IPostRepository : IUserRelatedRepository<Post>
    {
        Task Delete(Guid id);
    }
}

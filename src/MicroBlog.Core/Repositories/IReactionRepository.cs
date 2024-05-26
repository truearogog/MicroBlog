using MicroBlog.Core.Models;

namespace MicroBlog.Core.Repositories
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        Task DeleteForUser(string userId);
    }
}

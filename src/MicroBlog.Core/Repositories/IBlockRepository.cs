using MicroBlog.Core.Models;

namespace MicroBlog.Core.Repositories
{
    public interface IBlockRepository : IRepository<Block>
    {
        Task DeleteForUser(string userId);
    }
}

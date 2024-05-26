using MicroBlog.Core.Models;

namespace MicroBlog.Core.Repositories
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task DeleteForUser(string userId);
    }
}

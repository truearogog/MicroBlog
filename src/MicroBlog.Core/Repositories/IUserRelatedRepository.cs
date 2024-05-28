namespace MicroBlog.Core.Repositories
{
    public interface IUserRelatedRepository<T> : IRepository<T>
    {
        Task DeleteForUser(string userId);
    }
}

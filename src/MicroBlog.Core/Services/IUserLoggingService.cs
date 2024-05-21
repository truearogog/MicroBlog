namespace MicroBlog.Core.Services
{
    public interface IUserLoggingService
    {
        void AddOrUpdateLogin(string userId, DateTime dateTime);
        IReadOnlyDictionary<string, DateTime> GetLogins();
        void ResetLogins();
    }
}

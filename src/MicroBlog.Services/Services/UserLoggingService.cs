using MicroBlog.Core.Services;
using System.Collections.Concurrent;

namespace MicroBlog.Services.Services
{
    public class UserLoggingService : IUserLoggingService
    {
        private ConcurrentDictionary<string, DateTime> _userLogins = [];

        public void AddOrUpdateLogin(string userId, DateTime dateTime)
        {
            if (!_userLogins.ContainsKey(userId))
            {
                _userLogins.AddOrUpdate(userId, dateTime, (k, v) => dateTime);
            }
        }

        public IReadOnlyDictionary<string, DateTime> GetLogins()
        {
            return _userLogins.AsReadOnly();
        }

        public void ResetLogins()
        {
            if (_userLogins.Count > 100_000)
            {
                _userLogins = [];
            }
            else
            {
                _userLogins.Clear();
            }
        }
    }
}

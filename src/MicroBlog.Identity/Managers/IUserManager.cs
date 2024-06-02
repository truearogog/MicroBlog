using MicroBlog.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace MicroBlog.Identity.Managers
{
    public interface IUserManager
    {
        Task<IdentityResult> SetProfilePictureAsync(User user, string? profilePictureUrl);
        Task<string> GetProfilePictureUrlAsync(string userId);
        Task<IdentityResult> SetUserNameAsync(User user, string? userName);
        Task<string> GetUserNameAsync(string userId);
        Task<IdentityResult> SetLastSeenAsync(User user, DateTime dateTime);
        Task<IdentityResult> SetDescriptionAsync(User user, string description);
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
        Task<IEnumerable<User>> Search(Expression<Func<User, bool>> predicate, int skip, int take);
    }
}

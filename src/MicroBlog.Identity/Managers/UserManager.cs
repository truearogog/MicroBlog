using MicroBlog.Core.Constants;
using MicroBlog.Core.Services;
using MicroBlog.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace MicroBlog.Identity.Managers
{
    public class UserManager(
        IUserStore<User> store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<User> passwordHasher, 
        IEnumerable<IUserValidator<User>> userValidators, 
        IEnumerable<IPasswordValidator<User>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<UserManager<User>> logger, 
        ICacheService cacheService) : 
        UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger), IUserManager
    {
        private readonly ICacheService _cacheService = cacheService;

        public async Task<IdentityResult> SetProfilePictureAsync(User user, string? profilePictureUrl)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user);

            user.ProfilePictureUrl = profilePictureUrl;
            _cacheService.Remove(CacheNames.UserProfilePictureUrl + user.Id);
            await UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<string> GetProfilePictureUrlAsync(string userId)
        {
            ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(userId);

            var profilePictureUrl = await _cacheService.GetOrCreateAsync(CacheNames.UserProfilePictureUrl + userId, async entry =>
            {
                var user = await FindByIdAsync(userId);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return user?.ProfilePictureUrl ?? string.Empty;
            });

            return profilePictureUrl ?? string.Empty;
        }

        public override Task<IdentityResult> SetUserNameAsync(User user, string? userName)
        {
            _cacheService.Remove(CacheNames.Username + user.Id);
            return base.SetUserNameAsync(user, userName);
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(userId);

            var username = await _cacheService.GetOrCreateAsync(CacheNames.Username + userId, async entry =>
            {
                var user = await FindByIdAsync(userId);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return user?.UserName ?? string.Empty;
            });

            return username ?? string.Empty;
        }

        public async Task<IdentityResult> SetLastSeenAsync(User user, DateTime dateTime)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user);

            user.LastSeen = dateTime;
            await UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<IdentityResult> SetDescriptionAsync(User user, string description)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user);

            user.Description = description ?? string.Empty;
            await UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null)
        {
            ThrowIfDisposed();

            var query = Users;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(predicate);

            return await Users.AnyAsync(predicate);
        }

        public async Task<IEnumerable<User>> Search(Expression<Func<User, bool>> predicate, int skip, int take)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(predicate);

            return await Users.Where(predicate).Skip(skip).Take(take).ToListAsync();
        }
    }
}

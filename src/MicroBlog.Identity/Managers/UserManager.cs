﻿using MicroBlog.Core.Constants;
using MicroBlog.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        IMemoryCache memoryCache) : 
        UserManager<User>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<IdentityResult> SetProfilePictureAsync(User user, string? profilePictureUrl)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user);

            user.ProfilePictureUrl = profilePictureUrl;
            _memoryCache.Set(CacheNames.UserProfilePictureUrl + user.Id, profilePictureUrl);
            await UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<string> GetProfilePictureUrlAsync(string userId)
        {
            ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(userId);

            var profilePictureUrl = await _memoryCache.GetOrCreateAsync(CacheNames.UserProfilePictureUrl + userId, async entry =>
            {
                var user = await FindByIdAsync(userId);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return user?.ProfilePictureUrl ?? string.Empty;
            });

            return profilePictureUrl ?? string.Empty;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(userId);

            var username = await _memoryCache.GetOrCreateAsync(CacheNames.Username + userId, async entry =>
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
    }
}

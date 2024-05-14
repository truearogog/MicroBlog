using MicroBlog.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Identity.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<IdentityResult> SetProfilePictureAsync(this UserManager<User> userManager, User user, string? profilePictureUrl)
        {
            ArgumentNullException.ThrowIfNull(userManager);
            ArgumentNullException.ThrowIfNull(user);
            
            user.ProfilePictureUrl = profilePictureUrl;
            await userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
            return await userManager.UpdateAsync(user).ConfigureAwait(false);
        }
    }
}

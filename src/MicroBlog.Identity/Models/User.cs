using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Identity.Models
{
    public class User : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? LastSeen { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Identity.Models
{
    public class User : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; }
    }
}

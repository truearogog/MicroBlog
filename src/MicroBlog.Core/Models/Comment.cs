namespace MicroBlog.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public required Guid PostId { get; set; }
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public string UserProfilePictureUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}

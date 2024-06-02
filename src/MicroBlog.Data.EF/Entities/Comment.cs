#nullable disable

namespace MicroBlog.Data.EF.Entities
{
    public class Comment : IEquatable<Comment>
    {
        public Guid Id { get; set; }
        public required Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public bool Equals(Comment other)
        {
            return Id == other?.Id;
        }
    }
}

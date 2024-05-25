using MicroBlog.Core.Constants;

namespace MicroBlog.Core.Models
{
    public class Reaction
    {
        public required Guid PostId { get; set; }
        public required string UserId { get; set; }
        public required ReactionType Type { get; set; }
    }
}

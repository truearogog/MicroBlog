#nullable disable

using MicroBlog.Core.Constants;

namespace MicroBlog.Data.EF.Entities
{
    public class Reaction : IEquatable<Reaction>
    {
        public required Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public required string UserId { get; set; }
        public required ReactionType Type { get; set; }

        public bool Equals(Reaction other)
        {
            return PostId == other?.PostId;
        }
    }
}

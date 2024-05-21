namespace MicroBlog.Core.Models
{
    public class Block : IEquatable<Block>
    {
        public required string UserId { get; set; }
        public required string BlockedUserId { get; set; }

        public bool Equals(Block? other)
        {
            return other != null && UserId == other.UserId && BlockedUserId == other.BlockedUserId;
        }

        public override bool Equals(object obj) => Equals(obj as Block);
    }
}

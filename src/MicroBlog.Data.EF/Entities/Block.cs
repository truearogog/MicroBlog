namespace MicroBlog.Data.EF.Entities
{
    public class Block : IEquatable<Block>
    {
        public required string UserId { get; set; }
        public required string BlockedUserId { get; set; }

        public bool Equals(Block? other)
        {
            return UserId == other?.UserId && BlockedUserId == other?.BlockedUserId;
        }
    }
}

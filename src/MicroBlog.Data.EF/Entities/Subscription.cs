namespace MicroBlog.Data.EF.Entities
{
    public class Subscription : IEquatable<Subscription>
    {
        public required string ToUserId { get; set; }
        public required string FromUserId { get; set; }

        public bool Equals(Subscription? other)
        {
            return ToUserId == other?.ToUserId && FromUserId == other?.FromUserId;
        }
    }
}

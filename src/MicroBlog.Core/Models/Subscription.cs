namespace MicroBlog.Core.Models
{
    public class Subscription : IEquatable<Subscription>
    {
        public required string ToUserId { get; set; }
        public required string FromUserId { get; set; }

        public bool Equals(Subscription? other)
        {
            return other != null && ToUserId == other.ToUserId && FromUserId == other.FromUserId;
        }

        public override bool Equals(object obj) => Equals(obj as Subscription);
    }
}

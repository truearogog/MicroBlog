namespace MicroBlog.Data.EF.Entities
{
    public class Subscription
    {
        public required string ToUserId { get; set; }
        public required string FromUserId { get; set; }
    }
}

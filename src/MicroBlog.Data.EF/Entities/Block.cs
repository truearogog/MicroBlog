namespace MicroBlog.Data.EF.Entities
{
    public class Block
    {
        public required string UserId { get; set; }
        public required string BlockedUserId { get; set; }
    }
}

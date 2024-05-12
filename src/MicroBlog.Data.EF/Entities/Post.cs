namespace MicroBlog.Data.EF.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public required string UserId { get; set; }
    }
}

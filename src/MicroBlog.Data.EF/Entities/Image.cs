namespace MicroBlog.Data.EF.Entities
{
    public class Image
    {
        public required string Path { get; set; }
        public required string UserId { get; set; }
    }
}

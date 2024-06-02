namespace MicroBlog.Data.EF.Entities
{
    public class Image : IEquatable<Image>
    {
        public required string Path { get; set; }
        public required string UserId { get; set; }

        public bool Equals(Image? other)
        {
            return Path == other?.Path;
        }
    }
}

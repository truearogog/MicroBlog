#nullable disable

using System.ComponentModel.DataAnnotations;

namespace MicroBlog.Web.Models
{
    public class CreatePostModel
    {
        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Content { get; set; }
    }
}

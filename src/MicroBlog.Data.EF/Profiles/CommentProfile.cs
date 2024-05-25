using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Entities.Comment>();
            CreateMap<Comment, Entities.Comment>().ReverseMap();
        }
    }
}

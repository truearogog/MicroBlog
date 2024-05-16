using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, Entities.Post>();
            CreateMap<Post, Entities.Post>().ReverseMap();
        }
    }
}

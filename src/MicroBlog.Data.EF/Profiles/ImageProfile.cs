using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, Entities.Image>();
            CreateMap<Image, Entities.Image>().ReverseMap();
        }
    }
}

using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class ReactionProfile : Profile
    {
        public ReactionProfile()
        {
            CreateMap<Reaction, Entities.Reaction>();
            CreateMap<Reaction, Entities.Reaction>().ReverseMap();
        }
    }
}

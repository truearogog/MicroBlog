using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class BlockProfile : Profile
    {
        public BlockProfile()
        {
            CreateMap<Block, Entities.Block>();
            CreateMap<Block, Entities.Block>().ReverseMap();
        }
    }
}

using AutoMapper;
using MicroBlog.Data.EF.Profiles;

namespace MicroBlog.Data.EF.Tests
{
    internal class MapperHelpers
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(PostProfile));
            });
            return new Mapper(config);
        }
    }
}

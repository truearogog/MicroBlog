using AutoMapper;
using MicroBlog.Core.Models;

namespace MicroBlog.Data.EF.Profiles
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, Entities.Subscription>();
            CreateMap<Subscription, Entities.Subscription>().ReverseMap();
        }
    }
}

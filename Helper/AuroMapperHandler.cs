using AutoMapper;
using restaurant_app_API.Model;
using restaurant_app_API.Entity;

namespace restaurant_app_API.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<User, UserModel>().ForMember(item => item.StatusName, opt => opt.MapFrom(
                 item => item.IsActive ? "Active" : "In active")).ReverseMap();
            // .ForMember(dest => dest.Password, opt => opt.Ignore());

        }
    }
}
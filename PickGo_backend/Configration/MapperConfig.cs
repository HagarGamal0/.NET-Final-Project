using AutoMapper;
using PickGo_backend.DTOs.User;
using PickGo_backend.Models;

namespace PickGo_backend.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserRegisterDTO>().ReverseMap();



        }
    }
}

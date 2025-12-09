using AutoMapper;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.DTOs.User;
using PickGo_backend.DTOs.Request;     // <-- ADD THIS
using PickGo_backend.Models;

namespace PickGo_backend.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Existing maps
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<Supplier, SupplierRegisterDTO>().ReverseMap();
            CreateMap<Courier, CourierRegisterDTO>().ReverseMap();

            // NEW (required)
            CreateMap<RequestCreateDTO, Request>();
        }
    }
}

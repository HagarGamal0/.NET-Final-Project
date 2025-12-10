using AutoMapper;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.DTOs.User;
using PickGo_backend.Models;

namespace PickGo_backend.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<Supplier, SupplierRegisterDTO>().ReverseMap();
            CreateMap<Courier, CourierRegisterDTO>().ReverseMap();
            CreateMap<CourierCompleteProfileDTO, Courier>()
    .ForAllMembers(opts =>
        opts.Condition((src, dest, srcValue) =>
            srcValue != null)); // Update only non-null fields

            CreateMap<SupplierCompleteProfileDTO, Supplier>()
    .ForAllMembers(opts =>
        opts.Condition((src, dest, srcVal) => srcVal != null));




        }
    }
}

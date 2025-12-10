using AutoMapper;
using PickGo_backend.Models;
using PickGo_backend.DTOs.Request;
using PickGo_backend.DTOs.Package;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.DTOs.User;

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


            CreateMap<EditProfileDTO, User>()
    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // =======================
            //        REQUEST
            // =======================
            CreateMap<Request, RequestReadDTO>()
                .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => src.Packages));

            CreateMap<RequestCreateDTO, Request>();

            CreateMap<RequestUpdateDTO, Request>();


            // =======================
            //        PACKAGE
            // =======================
            CreateMap<Package, PackageReadDTO>();

            CreateMap<PackageCreateDTO, Package>();

            CreateMap<PackageUpdateDTO, Package>();

            // (Optional) if you want two-way mapping:
            // CreateMap<RequestReadDTO, Request>();
            // CreateMap<PackageReadDTO, Package>();

            CreateMap<Request, RequestReadDTO>().ReverseMap();
            CreateMap<RequestCreateDTO, Request>();
            CreateMap<RequestUpdateDTO, Request>();

            CreateMap<Package, PackageReadDTO>().ReverseMap();
            CreateMap<PackageCreateDTO, Package>();
            CreateMap<PackageUpdateDTO, Package>();

        }
    }
}

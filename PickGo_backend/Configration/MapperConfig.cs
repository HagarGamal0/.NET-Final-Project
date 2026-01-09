using AutoMapper;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Customer;
using PickGo_backend.DTOs.Package;
using PickGo_backend.DTOs.Request;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.DTOs.User;
using PickGo_backend.Models;

namespace PickGo_backend.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // =======================
            //        USER
            // =======================
            CreateMap<User, UserRegisterDTO>().ReverseMap();

            // =======================
            //        SUPPLIER
            // =======================
            CreateMap<Supplier, SupplierRegisterDTO>().ReverseMap();

            CreateMap<SupplierCompleteProfileDTO, Supplier>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcVal) => srcVal != null));

            CreateMap<Supplier, SupplierProfileDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address));

            // =======================
            //        COURIER
            // =======================
            CreateMap<Courier, CourierRegisterDTO>().ReverseMap();

            CreateMap<CourierCompleteProfileDTO, Courier>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcValue) => srcValue != null));

            CreateMap<CourierLocationDto, CourierLocation>()
                .ForMember(dest => dest.RecordedAt,
                    opt => opt.MapFrom(_ => DateTime.UtcNow));


            CreateMap<CourierRegisterDTO, Courier>();
            CreateMap<CourierCompleteProfileDTO, Courier>();


            // =======================
            //        REQUEST
            // =======================


            CreateMap<Request, RequestReadDTO>()
                .ForMember(dest => dest.Packages,
                    opt => opt.MapFrom(src => src.Packages));

            CreateMap<RequestCreateDTO, Request>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Packages, opt => opt.Ignore());

            CreateMap<RequestUpdateDTO, Request>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

           
            
            // =======================
            //        PACKAGE
            // =======================
            CreateMap<Package, PackageReadDTO>();

            CreateMap<PackageCreateDTO, Package>()
                .ForMember(dest => dest.RequestID, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CourierID, opt => opt.Ignore())
                .ForMember(dest => dest.DeliveredAt, opt => opt.Ignore())
                .ForMember(dest => dest.ShipmentRating, opt => opt.Ignore())
                .ForMember(dest => dest.ShipmentReviewID, opt => opt.Ignore());

            CreateMap<PackageUpdateDTO, Package>()
                .ForMember(dest => dest.RequestID, opt => opt.Ignore())
                .ForMember(dest => dest.CourierID, opt => opt.Ignore());


            // =======================
            //        CUSTOMER
            // =======================

            // Mapping with custom/nested properties
            CreateMap<Customer, CustomerDto>()
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
    .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages != null ? src.Packages.Count : 0))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)); // map from User.UserName

            // Simple DTOs can use ReverseMap
            CreateMap<CustomerJoinDto, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDto, Customer>().ReverseMap();
        }
    }
}

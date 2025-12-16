using AutoMapper;
using PickGo_backend.DTOs.User;
using PickGo_backend.DTOs.Supplier;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Request;
using PickGo_backend.DTOs.Package;
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
        }
    }
}

using AutoMapper;
using PickGo_backend.Models;
using PickGo_backend.DTOs.Request;
using PickGo_backend.DTOs.Package;

namespace PickGo_backend.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
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

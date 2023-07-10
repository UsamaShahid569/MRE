using System.Linq;
using AutoMapper;
using MRE.Contracts.Dtos;
using MRE.Domain.Entities;

namespace MRE.Profiles.Business
{
    public class BusinessAutoMapperProfile
    {
        public BusinessAutoMapperProfile()
        {
            //CreateMap<Business, BusinessDto>()
            //    .ForMember(dest => dest.Id,
            //        opts => opts.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.BusinessName,
            //        opts => opts.MapFrom(src => src.BusinessName))
            //    .ForMember(dest => dest.Address,
            //        opts => opts.MapFrom(src => src.Address))
            //    .ForMember(dest => dest.BusinessGroups,
            //        opts => opts.MapFrom(src => src.BusinessGroups))
            //    .ForMember(dest => dest.Contacts,
            //        opts => opts.MapFrom(src => src.Contacts));
        }
    }
}

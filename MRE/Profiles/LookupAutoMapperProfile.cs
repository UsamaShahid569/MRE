using System.Linq;
using AutoMapper;
using MRE.Contracts.Dtos;
using MRE.Domain.Entities;

namespace MRE.Api.Profiles
{
    public class LookupAutoMapperProfile : Profile
    {
        public LookupAutoMapperProfile()
        {
            CreateMap<Lookup, LookupDto>()
                .ForMember(dest => dest.Id,
                    opts => opts.MapFrom(des => des.Id))
                .ForMember(dest => dest.Name,
                    opts => opts.MapFrom(des => des.Name))
                .ForMember(dest => dest.LookupParentName,
                    opts => opts.MapFrom(des => des.LookupParent.Name));
        }
    }
}
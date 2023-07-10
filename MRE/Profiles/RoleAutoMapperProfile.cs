using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MRE.Contracts.Dtos;
using MRE.Domain.Entities;
using MRE.Domain.Entities.Identity;

namespace MRE.Profiles
{
    public class RoleAutoMapperProfile : Profile
    {
        public RoleAutoMapperProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Id,
                    opts => opts.MapFrom(des => des.Id))
                .ForMember(dest => dest.Name,
                    opts => opts.MapFrom(des => des.Name));
        }
    }
}
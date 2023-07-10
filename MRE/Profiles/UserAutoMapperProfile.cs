using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MRE.Contracts.Dtos;
using MRE.Domain.Entities.Identity;

namespace MRE.Profiles
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role,
                    opts => opts.MapFrom(des => string.Join(", ", des.UserRoles.Select(x => x.Role.Name).ToArray())));

            CreateMap<User, DropdownDto>().ForMember(a => a.Name,
                c => c.MapFrom(g => g.FullName));

            CreateMap<User, UserProfileDto>();
        }
    }
}

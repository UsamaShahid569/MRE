using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MRE.Contracts.Dtos;
using MRE.Contracts.Enums;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Context;


namespace MRE.Presistence.Concrete
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public RoleRepository(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<RoleDto> Get()
        {
            return _db.Roles.Where(a=>a.Name != RolesEnum.SuperAdmin).ProjectTo<RoleDto>(_mapper.ConfigurationProvider).ToList();
        }
    }
}

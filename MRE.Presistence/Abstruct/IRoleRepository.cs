using System;
using System.Collections.Generic;
using System.Text;
using MRE.Contracts.Dtos;
using MRE.Domain.Entities.Identity;

namespace MRE.Presistence.Abstruct
{
    public interface IRoleRepository
    {
        List<RoleDto> Get();
    }
}

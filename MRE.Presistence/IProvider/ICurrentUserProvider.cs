using MRE.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Presistence.IProvider
{
    public interface ICurrentUserProvider
    {
        Guid UserId { get; }
        string UserName { get; }
        string FullName { get; }
        string Email { get; }

        List<string> Roles { get; }
        Guid? TenantId { get; }

        bool IsAuthenticated { get; }
        bool IsSuperAdmin { get; }
    }
}

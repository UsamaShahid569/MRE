using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MRE.Contracts.Enums;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.IProvider;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MRE.Presistence.Providers
{
   public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IEnumerable<IHttpContextAccessor> httpContextAccessorArr)
        {
            _httpContextAccessor = httpContextAccessorArr.FirstOrDefault();
        }

        public Guid UserId => Data.UserId;

        public string UserName => Data.UserName;

        public string FullName => Data.FullName;

        public string Email => Data.Email;

        public List<string> Roles => Data.Roles;

        public Guid? TenantId => Data.TenantId;

        public bool IsAuthenticated => Data.IsAuthenticated;

        public bool IsSuperAdmin => Data.IsSuperAdmin;

        private CurrentUserModel _data;
        private CurrentUserModel Data => _data ?? (_data = GetFromHttpRequest());

        private CurrentUserModel GetFromHttpRequest()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return new CurrentUserModel
                {
                    IsAuthenticated = false
                };
            }
            else if (httpContext.User.Identity.IsAuthenticated == false)
            {
                return new CurrentUserModel
                {
                    IsAuthenticated = false
                };
            }
            else
            {
                var claims = httpContext.User;
                var currentUser = new CurrentUserModel
                {
                    IsAuthenticated = claims.Identity.IsAuthenticated,
                    Email = claims.FindFirst(x => x.Type == CustomClaimsEnum.Email)?.Value,
                    UserId = Guid.Parse(claims.FindFirst(x => x.Type == CustomClaimsEnum.UserId)?.Value),
                    UserName = claims.FindFirst(x => x.Type == ClaimTypes.Name)?.Value,
                    FullName = claims.FindFirst(x => x.Type == CustomClaimsEnum.FullName)?.Value,
                };

                if (claims.FindFirst(x => x.Type == CustomClaimsEnum.Roles) != null)
                {
                    currentUser.Roles = GetUserRoles();
                }

                var tenantId = claims.FindFirst(x => x.Type == CustomClaimsEnum.TenantId).Value;
                if (!String.IsNullOrEmpty(tenantId))
                {
                    currentUser.TenantId = Guid.Parse(tenantId);
                }

                var isSuperAdmin = claims.FindFirst(x => x.Type == CustomClaimsEnum.IsSuperAdmin).Value;
                if (!String.IsNullOrEmpty(isSuperAdmin))
                {
                    currentUser.IsSuperAdmin = bool.Parse(isSuperAdmin);
                }

                return currentUser;
            }

        }
        private List<string> GetUserRoles()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var roleNameArrJson = claims.Where(c => c.Type == CustomClaimsEnum.Roles).Select(x => x.Value).FirstOrDefault();

                if (roleNameArrJson != null)
                {
                    var roleNameArr = JsonConvert.DeserializeObject<List<string>>(roleNameArrJson);

                    //List<Role> userRoles = new List<Role>();
                    //foreach (var item in roleNameArr)
                    //{
                    //    Role role = (Role)Enum.Parse(typeof(Role), item, true);
                    //    userRoles.Add(role);
                    //}

                    return roleNameArr;
                }
            }

            return new List<string>();
        }
    }
}

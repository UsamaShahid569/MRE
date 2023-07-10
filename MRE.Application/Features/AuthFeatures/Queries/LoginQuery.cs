using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Enums;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Extensions;
using MRE.Presistence.IProvider;
using MRE.Presistence.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MRE.Domain.Entities.Identity;

namespace MRE.Application.Features.AuthFeatures.Queries
{
    public class LoginQuery : IRequest<CqrsResponse>
    {
        public LoginModel Model { get; set; }
        public LoginQuery(LoginModel model)
        {
            Model = model;
        }

        public class LoginQueryHandler : IRequestHandler<LoginQuery, CqrsResponse>
        {
            private readonly IAuthProvider _authProvider;

            public LoginQueryHandler( IAuthProvider authProvider, IUserRepository userRepository)
            {
                _authProvider = authProvider;
               
            }
            public async Task<CqrsResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
            {
                
                var user = await Task.Run(() => _authProvider.SignIn(request.Model));
                
                return user;
                
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MRE.Contracts.Enums;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.Context;
using MRE.Presistence.Extensions;
using MRE.Presistence.IProvider;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid.Helpers.Mail;
using Microsoft.EntityFrameworkCore;
using MRE.Contracts.Exceptions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Options;
using MRE.Presistence.Abstruct;
using Newtonsoft.Json;
using Google.Apis.Auth;

namespace MRE.Presistence.Providers
{
    public  class AuthProvider : IAuthProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<AuthSettingsModel> _authSettings;
        public AuthProvider(IUserRepository userRepository, 
            IOptions<AuthSettingsModel> authSettings)
        {
            _userRepository = userRepository;
            _authSettings = authSettings;
        }

        public async Task<CqrsResponse> SignIn(LoginModel model)
        {
            User user = await _userRepository.Query().Include(a=>a.UserRoles).ThenInclude(a=>a.Role)
                .SingleOrDefaultAsync(x => x.Email.Trim().ToLower() == model.Email.Trim().ToLower());

            if (user is null)
            {
                await Task.Run(() => _userRepository.AddLoginLog(new UserLogin
                {
                    Email = model.Email,
                    DateTime = DateTime.Now,
                    LoginResult = Domain.Enums.LoginResult.Failure,
                    ErrorMessage = "User not found."
                }));
                return new CqrsResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "User not found."
                };
            }
            string email = "";
            if (model.Type == LoginTypeEnum.Microsoft)
            {
                JwtSecurityToken token = ValidateMicrosoftIdToken(model.TokenId);
                email = token.Claims.SingleOrDefault(x => x.Type == "preferred_username").Value;
            }
            if (model.Type == LoginTypeEnum.Google)
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(model.TokenId, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _authSettings.Value.GoogleClientId }
                });
                email = payload.Email;
            }
            
            if (user.Email.Trim().ToLower() != email.Trim().ToLower())
            {
               await Task.Run(() => _userRepository.AddLoginLog(new UserLogin
                {
                   UserId = user.Id,
                   Email = model.Email,
                   DateTime = DateTime.Now,
                   LoginResult = Domain.Enums.LoginResult.Failure,
                   ErrorMessage = "Invalid token credentials."
               }));
                return new CqrsResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "Invalid token credentials."
                };
            }
            if (user.IsActive == false)
            {
                await Task.Run(() => _userRepository.AddLoginLog(new UserLogin
                {
                    UserId = user.Id,
                    Email = model.Email,
                    DateTime = DateTime.Now,
                    LoginResult = Domain.Enums.LoginResult.LockedOut,
                    ErrorMessage = "User is disabled."
                }));
                return new CqrsResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = "User is disabled."
                };
            }
            var roleNameArr = user.UserRoles.Select(x => x.Role.Name).ToList();
            var superAdmin = user.UserRoles.Any(x => x.Role.Name.Contains(RolesEnum.SuperAdmin));
            var claims = new List<Claim>
                    {
                        new Claim(CustomClaimsEnum.UserId , user.Id.ToString() ),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString() ),
                        new Claim(CustomClaimsEnum.FullName , user.FullName ),
                        new Claim(CustomClaimsEnum.Email , user.Email ),
                        new Claim(CustomClaimsEnum.IsSuperAdmin , superAdmin.ToString() ),
                        new Claim(CustomClaimsEnum.TenantId , user.TenantId.HasValue?user.TenantId.ToString():"" ),
                        new Claim(CustomClaimsEnum.Roles , JsonConvert.SerializeObject(roleNameArr))
                    };

            var expireDate = DateTime.UtcNow.AddMinutes(TokenExpirationTime());

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(null, null, claims, expires: expireDate, signingCredentials: creds);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            await Task.Run(() => _userRepository.AddLoginLog(new UserLogin
            {
                UserId = user.Id,
                Email = model.Email,
                DateTime = DateTime.Now,
                LoginResult = Domain.Enums.LoginResult.Success,
            }));
            return new GetUserTokenQueryResult
            {
                AccessToken = accessToken
            };
        }

        public JwtSecurityToken ValidateMicrosoftIdToken(string token)
        {
            string openIdConfigurationEndpoint = _authSettings.Value.OpenIdEndpoint;

            IConfigurationRetriever<OpenIdConnectConfiguration> configurationRetriever = new OpenIdConnectConfigurationRetriever();

            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(openIdConfigurationEndpoint, configurationRetriever);

            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = true
            };

            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            SecurityToken jwt;

            try
            {
                tokendHandler.ValidateToken(token, validationParameters, out jwt);
            }
            catch (Exception)
            {
                throw new InvalidTokenException("Invalid token.");
            }

            return jwt as JwtSecurityToken;
        }


        public int TokenExpirationTime()
        {
            DateTime midnight = DateTime.Today.AddDays(30);

            TimeSpan tokenExpirationTime = midnight - DateTime.Now;

            return Math.Abs((int)tokenExpirationTime.TotalMinutes);
        }
    }
    public class GetUserTokenQueryResult : CqrsResponse
    {
        public string AccessToken { get; set; }
    }
}

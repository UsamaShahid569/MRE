using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using Model.Models;

namespace MRE.Presistence.IProvider
{
    public interface IAuthProvider
    {
        Task<CqrsResponse> SignIn(LoginModel model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using Model.Models;

namespace MRE.Presistence.Abstruct
{
    public interface IUserRepository
    {
        void AddLoginLog(UserLogin model);
        void UpdateStatus(Guid id, bool status);
        IQueryable<User> Query();
        int Count(string search);
        UserModel Get(Guid userId);
        DataAndCountDto<UserDto> Get(int? skip, int? take, string search, string orderBy, Guid? roleId);
        bool ChangeStatus(Guid userId, bool IsActive);
        User Create(UserModel model);
        User Update(UserModel model);
        void Delete(Guid userId);
        void UpdateProfile(UserProfileModel model);
        UserProfileDto GetMemberProfile();
    }
}

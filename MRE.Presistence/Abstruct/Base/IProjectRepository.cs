using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Presistence.Abstruct.Base
{
    public interface IProjectRepository
    {
        User Create(UserModel model);
        User Update(UserModel model);
        void Delete(Guid userId);
    }
}

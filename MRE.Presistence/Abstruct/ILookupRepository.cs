using System;
using System.Collections.Generic;
using System.Text;
using MRE.Contracts.Dtos;
using MRE.Contracts.Filters;
using MRE.Contracts.Models;
using Model.Models;

namespace MRE.Presistence.Abstruct
{
    public interface ILookupRepository
    {
        DataAndCountDto<LookupDto> GetAll(LookupQueryFilter filter);
        LookupDto GetById(Guid lookupId);
        List<LookupDto> Get(String lookupParentName);
        LookupDto Get(string lookupName, string parentName);
        Guid Add(LookupModel model);
        void Update(LookupModel model);
        void Delete(Guid lookupId);

    }
}

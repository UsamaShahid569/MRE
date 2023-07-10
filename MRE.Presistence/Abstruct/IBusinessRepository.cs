using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Business;

namespace MRE.Presistence.Abstruct
{
    public interface IBusinessRepository
    {
        Business Create(BusinessModel model);
        void Delete(Guid id);
        Business Get(Guid id);
        List<Business> GetAllBussiness();
        Business Update(BusinessModel model);
    }
}
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Business;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Concrete.Base;
using MRE.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Presistence.Concrete
{
    public class BusinessRepository : ServiceBaseEntity<Business>, IBusinessRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public BusinessRepository(DataContext db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public Business Create(BusinessModel model)
        {

            var bussiness = new Business
            {
                BusinessName = model.BusinessName,
                Address = model.Address,
                BusinessGroups = model.BusinessGroups,
                Contacts = model.Contacts
            };

            _db.Add(bussiness);
            _db.SaveChanges();

            return bussiness;
        }

        public Business Get(Guid id)
        {
            return GetAll().Include(x => x.BusinessGroups)
                           .Include(x => x.Contacts)
                           .FirstOrDefault(x => x.Id == id);
        }

        public List<Business> GetAllBussiness()
        {
            return GetAll().Include(x => x.BusinessGroups)
                           .Include(x => x.Contacts)
                           .Where(x => x.Active)
                           .ToList();
        }

        public Business Update(BusinessModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Load the business
                    var bussiness = _db.Businesses
                                       .FirstOrDefault(x => x.Id == model.Id);

                    bussiness.BusinessName = model.BusinessName;
                    bussiness.Address = model.Address;

                    // Check if there are any exisitng BGs, delete/archive them
                    if (bussiness.BusinessGroups.Any())
                        _db.BusinessGroups.RemoveRange(bussiness.BusinessGroups.ToList());

                    // Any groups sent in request? Associate them with business
                    if (model.BusinessGroups.Any())
                        bussiness.BusinessGroups = model.BusinessGroups;


                    // Check if there are any exisitng contacts, delete/archive them
                    if (bussiness.Contacts.Any())
                        _db.Contacts.RemoveRange(bussiness.Contacts.ToList());
                     

                    // Any  contacts sent in request? Associate them with business
                    if (model.Contacts.Any())
                        bussiness.Contacts = model.Contacts;

                    _db.SaveChanges();

                    transaction.Commit();

                    return new Business
                    {
                        BusinessName = model.BusinessName,
                        Address = model.Address,
                        BusinessGroups = model.BusinessGroups,
                        Contacts = model.Contacts
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                    return new Business();
                }
            }

           
        }

        public void Delete(Guid id)
        {
            var business = this.Get(id);

            if (business != null)
            {
                business.Active = false;
                _db.SaveChanges();
            }
        }

    }
}

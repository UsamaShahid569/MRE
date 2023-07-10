using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MRE.Contracts.Dtos;
using MRE.Contracts.Enums;
using MRE.Contracts.Filters;
using MRE.Contracts.Models;
using MRE.Domain.Entities;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Context;
using MRE.Presistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace MRE.Presistence.Concrete
{
    public class LookupRepository : ILookupRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public LookupRepository(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public LookupDto GetById(Guid lookupId)
        {
            return _db.Lookups.Include(x => x.LookupParent).Where(x => x.Id == lookupId).ProjectTo<LookupDto>(_mapper.ConfigurationProvider).FirstOrDefault();
        }

        public DataAndCountDto<LookupDto> GetAll(LookupQueryFilter filter)
        {
            IQueryable<Lookup> query = _db.Lookups.
                Include(a=>a.LookupParent)
                .Where(a=>a.LookupParent.Name == filter.LookupParentName);
            if (!String.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x => x.Value.Contains(filter.Search)
                                         || x.Name.Contains(filter.Search));
            }



            if (!String.IsNullOrEmpty(filter.OrderBy))
            {
                var propertyName = filter.OrderBy.Split("-")[0];
                var isReverse = filter.OrderBy.Split("-")[1] == "desc" ? true : false;

                query = isReverse ? query.OrderByDescendingEx(propertyName) : query.OrderByEx(propertyName);
            }
            else
            {
                
            }
            var user = new DataAndCountDto<LookupDto>();
            user.Count = query.Count();
            if (filter.Skip != null)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take != null)
            {
                query = query.Take(filter.Take.Value);
            }

            user.Data = query.ProjectTo<LookupDto>(_mapper.ConfigurationProvider).ToList();

            return user;
        }

        public List<LookupDto> Get(string lookupParentName)
        {
            
                return _db.Lookups.Include(x => x.LookupParent).Where(x => x.LookupParent.Name == lookupParentName)
                    .ProjectTo<LookupDto>(_mapper.ConfigurationProvider).ToList();
            
        }

        public Guid Add(LookupModel model)
        {
            

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Lookup lookup =new Lookup();
                    var parent = _db.LookupParents.FirstOrDefault(x => x.Name == model.LookupParentName);
                    if (parent != null)
                    {
                         lookup = new Lookup
                         {
                             Name = model.Name,
                             Value = model.Value,
                             LookupParentId = parent.Id
                         };
                        _db.Lookups.Add(lookup);
                    }
                    _db.SaveChanges();

                    transaction.Commit();

                    return lookup.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return Guid.Parse("");
        }
        public void Update(LookupModel model)
        {

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var lookup = _db.Lookups.FirstOrDefault(a=>a.Id==model.Id.Value);
                    if (lookup != null)
                    {
                        lookup.Name = model.Name;
                        lookup.Value = model.Value;
                        var parent = _db.LookupParents.FirstOrDefault(a => a.Name == model.Name);
                        if (parent != null)
                        {
                            lookup.LookupParentId = parent.Id;
                        }
                    }
                    


                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void Delete(Guid lookupId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var lookup = _db.Lookups.Where(x => x.Id == lookupId).FirstOrDefault();

                    _db.Lookups.Remove(lookup);
                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public LookupDto Get(string lookupName, string parentName)
        {
            return _db.Lookups.Include(x => x.LookupParent)
                .Where(x => x.Name == lookupName && x.LookupParent.Name == parentName)
                .ProjectTo<LookupDto>(_mapper.ConfigurationProvider).FirstOrDefault();
        }
    }
}

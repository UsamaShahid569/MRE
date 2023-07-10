using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MRE.Domain.Entities.Base;
using MRE.Presistence.Context;


namespace MRE.Presistence.Concrete.Base
{
    public abstract class ServiceBaseEntity<T> where T : class, IDomainObject
    {
        private readonly DataContext _db;

        protected ServiceBaseEntity(DataContext db)
        {
            _db = db;
        }

        public virtual T FindById(Guid id, bool showArchive = false)
        {
            _db.ShowArchive = showArchive;
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _db.GetAll<T>();
        }
        public virtual IQueryable<T> GetAllWithoutQueryFilter()
        {
            return _db.GetAllWithoutQueryFilter<T>();
        }
       
        /// Maps a SqlDataReader record to an object.
       

    }
}

using MRE.Presistence.Abstruct.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MRE.Presistence.Concrete.Base
{
    public abstract class GenericBaseRepository<T> : IGeneric<T> where T : class
    {
        protected DbContext _dbContext { get; set; }

        public GenericBaseRepository(DbContext dBContext)
        {
            _dbContext = dBContext;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            Save();
        }

        public void AddRange(IEnumerable<T> entity) 
        {
            _dbContext.Set<T>().AddRange(entity);
            Save();
        }

        public void AddRangeAsync(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().AddRangeAsync(entity);
            Save();
        }
        public void EmptyTable()
        {
            var entities = _dbContext.Set<T>();
            _dbContext.Set<T>().RemoveRange(entities);
            Save();

        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            Save();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            Save();
        }

        public void DeleteAll(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
            Save();
        }

        public void DeleteUntill(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}

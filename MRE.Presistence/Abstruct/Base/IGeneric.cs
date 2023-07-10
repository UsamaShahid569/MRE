using System.Linq.Expressions;

namespace MRE.Presistence.Abstruct.Base
{
    public interface IGeneric<T>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void AddRangeAsync(IEnumerable<T> entity);
        void Update(T entity);
        void DeleteUntill(T entity);
        void Delete(T entity);
        void Save();
        void DeleteAll(List<T> entity);
        void EmptyTable();
    }
}

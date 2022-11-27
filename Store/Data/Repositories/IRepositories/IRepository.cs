using System.Linq.Expressions;

namespace Store.Data.Repositories.IRepositories
{
    public interface IRepository<T> where T:class
    {
        public T Find(int id);

        IEnumerable<T> FindAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null,
            bool isTracking = true);

        T FirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool isTracking = true);

        public void Add(T entity);
        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entyties);
        public void Save();
    }
}
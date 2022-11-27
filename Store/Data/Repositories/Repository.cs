using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Store.Data.Repositories.IRepositories;

namespace Store.Data.Repositories
{
    public class Repository<T>:IRepository<T> where T:class
    {
        private readonly AppDbContext _db;
        internal DbSet<T>? dbSet;

        public Repository(AppDbContext db)
        {
            _db= db;
            dbSet=db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet!.Add(entity);
        }

        public T Find(int id) => dbSet!.Find(id)!;

        public IEnumerable<T> FindAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T>? query = dbSet;

            if (filter!=null)
            {
                query=query!.Where(filter);
            }

            if (includeProperties!=null)
            {
                foreach (var includePropperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query!.Include(includePropperty);
                }
            }

            if (orderBy!=null)
            {
                query=orderBy(query!);
            }

            if (!isTracking)
            {
                query=query!.AsNoTracking();
            }

            return query!.ToList();
        }

        public T FirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<T>? query = dbSet;

            if (filter!=null)
            {
                query=query!.Where(filter);
            }

            if (includeProperties!=null)
            {
                foreach (var includePropperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query!.Include(includePropperty);
                }
            }

            if (!isTracking)
            {
                query=query!.AsNoTracking();
            }

            return query!.FirstOrDefault()!;
        }

        public void Remove(T entity)
        {
            dbSet!.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entyties)
        {
            dbSet!.RemoveRange(entyties);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
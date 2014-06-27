using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected IDbContext Context;
        protected IDbSet<TEntity> Set;

        public Repository(IDbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public TEntity Find(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Set.Attach(entity);
        }

        public void Remove(params object[] keyValues)
        {
            var entity = Find(keyValues);
            if (entity != null) Set.Remove(entity);
        }

        public IQueryable<TEntity> Queryable()
        {
            return Set.AsQueryable();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }


        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
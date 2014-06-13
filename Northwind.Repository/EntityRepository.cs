using System;
using System.Linq;

namespace Northwind.Repository
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public EntityRepository(IDbContext context)
        {
            
        }
        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Queryable()
        {
            throw new NotImplementedException();
        }
    }
}
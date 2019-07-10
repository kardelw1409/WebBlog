using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.Infrastructure.DbContexts;

namespace WebBlog.Infrastructure.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        BlogDbContext dbContext;
        public EntityRepository(BlogDbContext contex)
        {
            dbContext = contex;
        }

        public async Task Create(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
        }

        public Task<TEntity> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}

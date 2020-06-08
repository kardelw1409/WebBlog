using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.DbContexts;
using System.Linq;

namespace WebBlog.ApplicationCore.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected BlogDbContext dbContext;
        public RepositoryBase(BlogDbContext contex)
        {
            dbContext = contex;
        }
        public async Task<int> Create(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<TEntity> Remove(int? id)
        {
            var entityItem = await FindById(id);
            dbContext.Set<TEntity>().Remove(entityItem);
            await dbContext.SaveChangesAsync();
            return entityItem;
        }

        public async Task<TEntity> FindById(int? id)
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => dbContext.Set<TEntity>().Where(predicate));
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await dbContext.Set<TEntity>().CountAsync();
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }

    }
}

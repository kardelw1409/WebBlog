using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.ApplicationCore.DbContexts;
using System.Linq;

namespace WebBlog.ApplicationCore.Repositories
{
    public abstract class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected BlogDbContext dbContext;
        public EntityRepository(BlogDbContext contex)
        {
            dbContext = contex;
        }
        public async Task<int?> Create(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id; 
        }

        public async Task<TEntity> Remove(int? id)
        {
            var entityItem = await FindById(id);
            if (entityItem == null)
            {
                throw new NullReferenceException("Data not find");
            }
            dbContext.Set<TEntity>().Remove(entityItem);
            await dbContext.SaveChangesAsync();
            return entityItem;
        }

        public async Task<TEntity> FindById(int? id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            return await Task.FromResult(dbContext.Set<TEntity>().Where(predicate));
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            var entityItem = await dbContext.Set<TEntity>().SingleOrDefaultAsync(p => p.Id == entity.Id);
            if (entityItem != null)
            {
                dbContext.Entry(entityItem).CurrentValues.SetValues(entity);
                await dbContext.SaveChangesAsync();
            }

        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

    }
}

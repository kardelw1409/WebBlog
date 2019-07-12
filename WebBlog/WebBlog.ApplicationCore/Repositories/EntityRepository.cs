using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public abstract class EntityRepository<TEntity> : IMainRepository<TEntity>
        where TEntity : Entity
    {
        protected BlogDbContext dbContext;
        public EntityRepository(BlogDbContext contex)
        {
            dbContext = contex;
        }
        public async Task Create(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Remove(int id)
        {
            /*var entityItem = DbContext.Set<TEntity>().Where(p => p.Id == entity.Id).FirstOrDefault();
            if (entityItem != null)
            {
                DbContext.Set<TEntity>().Remove(entityItem);
                await DbContext.SaveChangesAsync();
            }*/
            var entityItem = await FindById(id);
            if (entityItem == null)
            {
                throw new NullReferenceException("Data not find");
            }
            dbContext.Set<TEntity>().Remove(entityItem);
            await dbContext.SaveChangesAsync();
            return entityItem;
        }

        public async Task<TEntity> FindById(int id)
        {
            return await dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

    }
}

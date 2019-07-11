using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public abstract class FullEntityRepository<TEntity> : EntityRepository<TEntity>, IAdditionalRepository<TEntity>
        where TEntity : Entity
    {

        public FullEntityRepository(BlogDbContext contex) : base(contex)
        {

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

        public async Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            return await Task.FromResult(dbContext.Set<TEntity>().Where(predicate));
        }

    }
}

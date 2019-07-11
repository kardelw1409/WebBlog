using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Interfaces
{
    public interface IAdditionalRepository<TEntity>
        where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate);
        Task Update(TEntity entity);
    }
}

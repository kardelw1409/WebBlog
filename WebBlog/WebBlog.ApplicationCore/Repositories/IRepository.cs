using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Repositories
{
    public interface IRepository<TEntity> : IDisposable 
        where TEntity : Entity
    {
        Task<int> Create(TEntity entity);
        Task<TEntity> FindById(int? id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Remove(int? id);
        Task<IEnumerable<TEntity>> Get(Func<TEntity, bool> predicate);
        Task Update(TEntity entity);
        Task<int> Count();

    }
}

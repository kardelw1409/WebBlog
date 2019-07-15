using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Interfaces
{
    public interface IMainRepository<TEntity> : IDisposable 
        where TEntity : Entity
    {
        Task Create(TEntity entity);
        Task<TEntity> FindById(int? id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Remove(int? id);

    }
}

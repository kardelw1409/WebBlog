using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Interfaces
{
    public interface IFullRepository<TEntity> : IAdditionalRepository<TEntity>, IMainRepository<TEntity>
        where TEntity : Entity
    {

    }
}

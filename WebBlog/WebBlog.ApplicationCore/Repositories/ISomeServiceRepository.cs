﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Repositories
{
    public interface ISomeServiceRepository<TEntity>
        where TEntity : Entity
    {
        Task<TEntity> GetData(string ip);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class AccountImageRepository : EntityRepository<AccountImage>
    {
        public AccountImageRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

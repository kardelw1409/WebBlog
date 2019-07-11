using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public class AvatarImageRepository : EntityRepository<AvatarImage>
    {
        public AvatarImageRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

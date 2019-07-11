using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public class PostRepository : FullEntityRepository<Post>
    {
        public PostRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

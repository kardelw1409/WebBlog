using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public class AuthorRepository : FullEntityRepository<Author>
    {
        public AuthorRepository(BlogDbContext context) : base(context)
        {
        }
    }
}

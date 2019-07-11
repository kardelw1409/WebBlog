using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public class CommentToCommentRepository : EntityRepository<CommentToComment>
    {
        public CommentToCommentRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

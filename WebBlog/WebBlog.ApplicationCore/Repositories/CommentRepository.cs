using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.DbContexts;

namespace WebBlog.ApplicationCore.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>
    {
        public CommentRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

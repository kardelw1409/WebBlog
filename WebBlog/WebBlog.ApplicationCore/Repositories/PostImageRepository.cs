using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class PostImageRepository : EntityRepository<PostImage>
    {
        public PostImageRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

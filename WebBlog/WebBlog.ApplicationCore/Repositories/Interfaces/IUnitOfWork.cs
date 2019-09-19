using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<Comment> CommentRepository { get; }

    }
}

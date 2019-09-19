using System;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        BlogDbContext dbContext;
        IRepository<Category> categoryRepository;
        IRepository<Post> postRepository;
        IRepository<Comment> commentRepository;

        public UnitOfWork(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoryRepository(dbContext);
                }
                return categoryRepository;
            }
        }

        public IRepository<Post> PostRepository
        {
            get
            {
                if (postRepository == null)
                {
                    postRepository = new PostRepository(dbContext);
                }
                return postRepository;
            }
        }

        public IRepository<Comment> CommentRepository
        {
            get
            {
                if (commentRepository == null)
                {
                    commentRepository = new CommentRepository(dbContext);
                }
                return commentRepository;
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                dbContext.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

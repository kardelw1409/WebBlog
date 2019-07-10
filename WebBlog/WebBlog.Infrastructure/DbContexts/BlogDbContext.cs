using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.Infrastructure.DbContexts
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        public DbSet<AvatarImage> AvatarImages { get; set; }
        public DbSet<CommentToComment> CommentToComments { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<CommentOfPost> CommentOfPosts { get; set; }
        public DbSet<Post> Posts { get; set; }
 
    }
}

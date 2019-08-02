using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.DbContexts
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        public DbSet<CommentToComment> CommentToComments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CommentOfPost> CommentOfPosts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    CategoryName = "Sport",
                    Discription = "A sport is commonly defined as an athletic activity that " +
                "involves a degree of competition, such as tennis or basketball. " +
                "Some games and many kinds of racing are called sports. " +
                "A professional at a sport is called an athlete. Many people play sports with their friends."
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Nature",
                    Discription = "Natural World. Natural World refers to the 'Natural Environment' " +
                    "that surrounds us in various forms like the earth, sun, moon, stars, forests, rivers, " +
                    "animals etc. It can be described as the Earth's environment which includes everything " +
                    "which is not man made and / or which has not been substantially altered by Humans."
                });
        }

    }
}

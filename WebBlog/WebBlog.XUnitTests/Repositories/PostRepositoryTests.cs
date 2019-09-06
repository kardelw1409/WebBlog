using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using Xunit;

namespace WebBlog.XUnitTests
{
    public class PostRepositoryTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPosts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "SomeDatabase")
                .Options;
            var context = new BlogDbContext(options);
            var repository = new PostRepository(context);
            foreach (var count in GetTestPosts())
            {
                await repository.Create(count);
            }
            // Act
            var resultPosts = await repository.GetAll();
            var resultList = new List<Post>(resultPosts);
            // Assert
            Assert.Equal(2, resultList.Count);
        }

        private List<Post> GetTestPosts()
        {
            var testPosts = new List<Post>();
            testPosts.Add(new Post()
            {
                Title = "Name First",
                CategoryId = 1,
                Content = "Content First",
                CreationTime = DateTime.Now,
                PostImage = new byte[0],
                HasImage = false,
                LastModifiedTime = DateTime.Now,
                IsConfirmed = true,
                UserId = "user id"
            });
            testPosts.Add(new Post()
            {
                Title = "Name Second",
                CategoryId = 1,
                Content = "Content Second",
                CreationTime = DateTime.Now,
                PostImage = new byte[0],
                HasImage = false,
                LastModifiedTime = DateTime.Now,
                IsConfirmed = true,
                UserId = "user_id"
            });
            return testPosts;
        }

    }
}

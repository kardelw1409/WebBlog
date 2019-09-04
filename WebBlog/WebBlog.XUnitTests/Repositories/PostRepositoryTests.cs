using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Controllers;
using WebBlog.Web.Models;
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
                PostImage = LoadDefaultImage(),
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
                PostImage = LoadDefaultImage(),
                HasImage = false,
                LastModifiedTime = DateTime.Now,
                IsConfirmed = true,
                UserId = "user_id"
            });
            return testPosts;
        }

        private byte[] LoadDefaultImage()
        {
            byte[] imageData = null;
            var file = new FileStream($"C:/Users/Valery_Kardel/source/repos/WebBlog/WebBlog/WebBlog.Web/wwwroot/images/post.png", FileMode.Open);
            var length = file.Length;

            using (var binaryReader = new BinaryReader(file))
            {
                imageData = binaryReader.ReadBytes((int)length);
            }
            return imageData;
        }
    }
}

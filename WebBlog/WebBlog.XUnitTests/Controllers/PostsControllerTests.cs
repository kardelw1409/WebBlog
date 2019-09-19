using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Controllers;
using Xunit;

namespace WebBlog.XUnitTests.Controllers
{
    public class PostsControllerTests
    {
        [Fact]
        public async void Index_IfCallAction_ReturnNotNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "SomeDatabase")
                .Options;
            var context = new BlogDbContext(options);
            var mockUserManager = new FakeUserManager();
            var unit = new UnitOfWork(context);
            var controller = new PostsController(mockUserManager, unit);
            // Act
            var result = await controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }
    }
}

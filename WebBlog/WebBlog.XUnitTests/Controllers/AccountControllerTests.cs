using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.IO;
using System.Security.Claims;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Controllers;
using WebBlog.Web.Models;
using Xunit;

namespace WebBlog.XUnitTests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async void IndexUserPosts_IfCallAction_ReturnNotNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "SomeDatabase")
                .Options;
            var context = new BlogDbContext(options);
            var mockUserManager = new FakeUserManager();
            var unit = new UnitOfWork(context);
            var controller = new AccountController(mockUserManager, unit);
            // Act
            var result = await controller.IndexUserPosts("1") as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ChangeAvatar_IfCallAction_ReturnNotNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "SomeDatabase")
                .Options;
            var context = new BlogDbContext(options);
            var mockUserManager = new FakeUserManager();
            var unit = new UnitOfWork(context);
            var controller = new AccountController(mockUserManager, unit);
            // Act
            var result = controller.ChangeAvatar() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void ChangeAvatar_IfUploadImage_ReturnNotNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "SomeDatabase")
                .Options;
            var context = new BlogDbContext(options);
            var mockUserManager = new Mock<FakeUserManager>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }));

            var userApp = new ApplicationUser() { Id = "1", UserName = "User", Email = "user@gmail.com" };
            mockUserManager
                .Setup(_ => _.GetUserAsync(user))
                .ReturnsAsync(userApp);
            var unit = new UnitOfWork(context);

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var controller = new AccountController(mockUserManager.Object, unit);
            var file = fileMock.Object;
            var image = new ImageViewModel()
            {
                AvatarImage = file
            };

            //Act
            var result = await controller.ChangeAvatar(image);

            //Assert
            Assert.IsType<IActionResult>(result);
        }

    }
}

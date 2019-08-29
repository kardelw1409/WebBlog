using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IRepository<Post> postRepository;

        public AccountController(UserManager<ApplicationUser> userManager, IRepository<Post> postRepository)
        {
            this.userManager = userManager;
            this.postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> IndexUserPosts(string id)
        {
            var postList = await postRepository.Get(i => i.UserId == id);
            return View(postList);
        }

        [HttpGet]
        public IActionResult ChangeAvatar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar([Bind("AvatarImage")]ImageViewModel image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(image.AvatarImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)image.AvatarImage.Length);
                    }
                    var user = userManager.GetUserAsync(User).Result;
                    user.AccountImage = imageData;

                    await userManager.UpdateAsync(user);

                    return Redirect("~/Identity/Account/Manage");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View();
        }
    }
}
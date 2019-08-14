using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> ChangeAvatar(ImageViewModel image)
        {
            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(image.AvatarImage.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)image.AvatarImage.Length);
            }
            var user = userManager.GetUserAsync(User).Result;
            user.AccountImage = imageData;
            if (ModelState.IsValid)
            {
                try
                {
                    await userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return Redirect("~/Identity/Account/Manage");
        }
    }
}
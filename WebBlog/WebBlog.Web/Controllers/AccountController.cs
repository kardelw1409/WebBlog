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
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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
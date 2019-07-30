using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Entities.AbstractEntities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize]
    public class AccountImageController : Controller
    {
        private readonly IRepository<AccountImage> accountImageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountImageController(IRepository<AccountImage> accountImageRepository, UserManager<ApplicationUser> userManager)
        {
            this.accountImageRepository = accountImageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var idList = new List<int?>();
            var imageList = await accountImageRepository.GetAll();
            foreach (var count in imageList)
            {
                idList.Add(count.Id);
            }
            return View(idList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile picture)
        {
            if (picture == null || picture.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream memoryStream = new MemoryStream();
                await picture.OpenReadStream().CopyToAsync(memoryStream);
                var image = Image.FromStream(memoryStream);
                var imageEntity = new AccountImage()
                {
                    Name = picture.Name,
                    Data = memoryStream.ToArray(),
                    Width = image.Width,
                    Height = image.Height,
                    ContentType = picture.ContentType
                };
                await accountImageRepository.Create(imageEntity);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChooseImage(int? id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            user.AccountImageId = id;
            await userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewImage(int? id)
        {
            var image = await accountImageRepository.FindById(id);
            var ms = new MemoryStream(image.Data);

            return new FileStreamResult(ms, image.ContentType);
        }

        // GET: AccountImage/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await accountImageRepository.FindById(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        // POST: CommentOfPosts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            await accountImageRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
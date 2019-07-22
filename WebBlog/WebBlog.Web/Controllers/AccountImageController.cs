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
        private readonly IMainRepository<AccountImage> accountImageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountImageController(IMainRepository<AccountImage> accountImageRepository, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> UploadImage(IList<IFormFile> files)
        {
            IFormFile uploadedImage = files.FirstOrDefault();
            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream memoryStream = new MemoryStream();
                await uploadedImage.OpenReadStream().CopyToAsync(memoryStream);
                Image image = Image.FromStream(memoryStream);
                var imageEntity = new AccountImage()
                {
                    Name = uploadedImage.Name,
                    Data = memoryStream.ToArray(),
                    Width = image.Width,
                    Height = image.Height,
                    ContentType = uploadedImage.ContentType
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
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.Web.Controllers
{
    public class AccountImageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile uploadedImage)
        {
            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                MemoryStream memoryStream = new MemoryStream();
                await uploadedImage.CopyToAsync(memoryStream);

                user.AvatarImage = memoryStream.ToArray();

            }
            return RedirectToAction("Index");
        }

        /*[HttpGet]
        public async FileStreamResult ViewUserImage()
        {

                var user = await userManager.GetUserAsync(HttpContext.User);
                var image = user.AvatarImage;
                MemoryStream memoryStream = new MemoryStream(image);

                return new FileStreamResult(memoryStream, 
          
        }*/
    }
}
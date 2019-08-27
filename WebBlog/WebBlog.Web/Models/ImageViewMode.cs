using Microsoft.AspNetCore.Http;
using WebBlog.ApplicationCore.Attributes;

namespace WebBlog.Web.Models
{
    public class ImageViewModel
    {
        [ValidateImage]
        public IFormFile AvatarImage { get; set; }

    }
}

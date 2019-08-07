using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Attributes;

namespace WebBlog.Web.Models
{
    public class ImageViewModel
    {
        [ValidateImage]
        [Display(Name = "PostImage")]
        public IFormFile AvatarImage { get; set; }

    }
}

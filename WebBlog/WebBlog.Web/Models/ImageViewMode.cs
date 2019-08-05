using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlog.Web.Models
{
    public class ImageViewModel
    {
        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "PostImage")]
        public IFormFile AvatarImage { get; set; }

    }
}

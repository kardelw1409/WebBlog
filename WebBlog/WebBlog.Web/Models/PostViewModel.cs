using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Attributes;

namespace WebBlog.Web.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Content")]
        [Display(Name = "Content")]
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime LastModifiedTime { get; set; }

        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "PostImage")]
        public IFormFile PostImage { get; set; }
        public int CategoryId { get; set; }
    }
}

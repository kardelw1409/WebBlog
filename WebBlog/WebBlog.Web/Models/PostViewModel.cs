using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Attributes;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.Web.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte[] PostImage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlog.Web.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public IFormFile PostImage { get; set; }
        public int CategoryId { get; set; }
    }
}

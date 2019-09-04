using System;
using System.ComponentModel.DataAnnotations;

namespace WebBlog.Web.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte[] PostImage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}

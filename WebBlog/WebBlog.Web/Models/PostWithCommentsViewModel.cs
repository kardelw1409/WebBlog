using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.Web.Models
{
    public class PostWithCommentsViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; } 
        public Comment Comment { get; set; }
    }
}

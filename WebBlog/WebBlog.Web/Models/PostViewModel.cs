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
        public DateTime UpdateTime { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
    }
}

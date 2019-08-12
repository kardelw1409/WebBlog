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
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }

        public Comment Comment { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class PostImage : ImageOfApp
    {
        public byte[] ResizeImage { get; set; }

        public ICollection<Post> Posts { get; set; } 
    }
}

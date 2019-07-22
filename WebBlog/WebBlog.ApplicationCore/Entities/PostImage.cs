using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class PostImage : ImageEntity
    {
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}

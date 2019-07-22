using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.ApplicationCore.Entities.AbstractEntities
{
    public class AccountImage : ImageEntity
    {
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } 
    }
}

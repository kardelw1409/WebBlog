using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }

        public int? AccountImageId { get; set; }
        public AccountImage AccountImage { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public int PriorityCategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}

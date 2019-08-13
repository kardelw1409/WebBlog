using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBlog.ApplicationCore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }

        public byte[] AccountImage { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}

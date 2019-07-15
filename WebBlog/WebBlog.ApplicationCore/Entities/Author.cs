using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Author : IdentityUser
    {
        public override string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public int PriorityCategoryId { get; set; }
        public Category Category { get; set; }
        

        public int AvatarImageId { get; set; }
        public AvatarImage AvatarImage { get; set; }


        public ICollection<Post> Posts { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Category : Entity
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Discription { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
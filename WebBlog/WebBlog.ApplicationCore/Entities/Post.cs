using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebBlog.ApplicationCore.Attributes;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Post : Entity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastModifiedTime { get; set; }

        public bool HasImage { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public byte[] PostImage { get; set; }
        [ValidateImage]
        [Display(Name = "PostImage")]
        [NotMapped]
        public IFormFile FormPostImage { get; set; }
        [NotMapped]
        public string ImageData { get; set; }

        // This property is needed for post verification. 
        // When you create a post, it will be false. 
        // Next, the administrator will have to confirm this post for publication to all users.
        public bool IsConfirmed { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}

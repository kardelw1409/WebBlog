using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Comment : Entity
    {
        [Required]
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int? ParentId { get; set; }
        public virtual Comment Parent { get; set; }

        public virtual ICollection<Comment> Children { get; set; }

    }
}

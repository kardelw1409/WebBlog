using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Comments : Entity
    {
        [Required]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int? ParentCommentId { get; set; }
        public virtual Comments ParentComment { get; set; }

        public virtual ICollection<Comments> Children { get; set; }

    }
}

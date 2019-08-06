using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebBlog.ApplicationCore.Attributes;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Post : Entity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastModifiedTime { get; set; }
        public byte[] PostImage { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<CommentOfPost> CommentsOfPost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class CommentToComment : Comment
    {
        public int CommentOfPostId { get; set; }
        public virtual CommentOfPost CommentOfPost { get; set; }
    }
}

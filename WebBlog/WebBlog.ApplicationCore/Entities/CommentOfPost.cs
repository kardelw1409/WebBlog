using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class CommentOfPost : Comment
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<CommentToComment> CommentToComments { get; set; }

    }
}

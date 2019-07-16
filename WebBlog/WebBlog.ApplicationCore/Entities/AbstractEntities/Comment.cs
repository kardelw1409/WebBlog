using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.ApplicationCore.Entities.AbstractEntities
{
    public abstract class Comment : Entity
    {
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

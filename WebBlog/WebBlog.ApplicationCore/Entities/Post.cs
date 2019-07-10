using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Post : Entity
    {
        public string Title { get; set; }

        public string Content { get; set; }
        
        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int PostImageId { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

    }
}

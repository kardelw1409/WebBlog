using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.Infrastructure.DTO
{
    public class PostDTO
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
        public DateTime UpdateTime { get; set; }
        public byte[] PostImage { get; set; }
        public byte[] ResizeImage { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CommentOfPost> CommentOfPosts { get; set; }
    }
}

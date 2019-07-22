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
        
        public int? PostImageId { get; set; }
        public PostImage PostImage { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CommentOfPost> CommentOfPosts { get; set; }
    }
}

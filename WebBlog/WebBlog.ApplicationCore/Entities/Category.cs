using System.Collections.Generic;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }

        public string Discraption { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
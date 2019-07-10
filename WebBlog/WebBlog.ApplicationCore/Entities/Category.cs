using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }

        public string Discraption { get; set; }
    }
}
using System.Collections.Generic;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Infrastructures
{
    public class PostsComparer : IComparer<Post>
    {
        public int Compare(Post x, Post y)
        {
            return y.LastModifiedTime.CompareTo(x.LastModifiedTime);
        }
    }
}

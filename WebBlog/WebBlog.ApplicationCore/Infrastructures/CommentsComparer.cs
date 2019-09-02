using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Infrastructures
{
    public class CommentsComparer : IComparer<Comment>
    {
        public int Compare(Comment first, Comment second)
        {
            return second.CreationTime.CompareTo(first.CreationTime);
        }
    }
}

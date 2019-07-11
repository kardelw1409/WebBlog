using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.Infrastructure.DTO;

namespace WebBlog.Infrastructure.Interfaces
{
    public interface IPostService
    {

        void MakePost(PostDTO postDto);
    }
}

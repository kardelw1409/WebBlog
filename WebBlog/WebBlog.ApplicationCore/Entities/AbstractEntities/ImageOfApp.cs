using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.ApplicationCore.Entities.AbstractEntities
{
    public abstract class ImageOfApp : Entity
    {
        public byte[] Image { get; set; }
    }
}

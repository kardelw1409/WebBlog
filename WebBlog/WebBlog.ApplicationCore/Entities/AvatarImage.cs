using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class AvatarImage : Entity
    {
        public byte[] Image { get; set; }
    }
}

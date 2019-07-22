using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.ApplicationCore.Entities.AbstractEntities
{
    public abstract class ImageEntity : Entity
    {
        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WebBlog.Infrastructure.DTO
{
    public class AuthorDTO
    {
        public string Nickname { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public int PriorityCategoryId { get; set; }

        public int AvatarImageId { get; set; }
    }
}

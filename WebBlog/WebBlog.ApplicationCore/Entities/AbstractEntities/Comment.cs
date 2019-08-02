﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebBlog.ApplicationCore.Entities.AbstractEntities
{
    public abstract class Comment : Entity
    {
        [Required]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

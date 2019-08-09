﻿using System;
using System.Collections.Generic;
using System.Text;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>
    {
        public CategoryRepository(BlogDbContext contex) : base(contex)
        {
        }
    }
}

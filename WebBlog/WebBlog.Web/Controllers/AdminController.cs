using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IRepository<Post> postRepository;
        
        public AdminController(IRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task<IActionResult> IndexUnverifiedPosts()
        {
            var postList = await postRepository.Get(i => i.IsConfirmed == false);
            //ViewData["Category"] = await categoryRepository.GetAll();
            var postViewList = postList.Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                PostImage = p.PostImage,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                CategoryName = p.Category.CategoryName,
                CreationTime = p.CreationTime,
                LastModifiedTime = p.LastModifiedTime,
                UserName = p.User.UserName
            });
            return View(postViewList);
        }


    }
}

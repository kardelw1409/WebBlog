﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Infrastructures;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IUnitOfWork unitOfWork;
        UserManager<ApplicationUser> userManager;
        
        public AdminController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> IndexUsers()
        {
            var tempAccount = await userManager.GetUserAsync(User);

            var users = userManager.Users.Where(u => u != tempAccount);
            return View(users);
        }

        public async Task<IActionResult> IndexUnverifiedPosts()
        {
            var postList = (await unitOfWork.PostRepository.Get(i => i.IsConfirmed == false)).ToList();
            postList.Sort(new PostsComparer());

            var postViewList = postList.Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                PostImage = p.PostImage,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                CategoryName = p.Category?.CategoryName,
                CreationTime = p.CreationTime,
                LastModifiedTime = p.LastModifiedTime,
                UserName = p.User.UserName
            }).ToList();

            return View(postViewList);
        }

        public async Task<IActionResult> ShowUserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(user);
            ViewData["Roles"] = roles; 
            return View(user);
        }


        public async Task<IActionResult> DeleteRoles(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);

            return View(user);
        }

        public async Task<IActionResult> AddUserRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            await userManager.AddToRoleAsync(user, "User");

            return RedirectToAction(nameof(IndexUsers));
        }

        public async Task<IActionResult> AddAdminRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);
            await userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction(nameof(IndexUsers));
        }

        [HttpPost, ActionName("DeleteRoles")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, roles.ToArray());

            return RedirectToAction(nameof(IndexUsers));
        }
    }
}

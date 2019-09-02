﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Infrastructures;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    public class PostsController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IRepository<Post> postRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<Comment> commentRepository;

        public PostsController(UserManager<ApplicationUser> userManager, IRepository<Post> postRepository,
            IRepository<Category> categoryRepository, IRepository<Comment> commentRepository)
        {
            this.userManager = userManager;
            this.postRepository = postRepository;
            this.categoryRepository = categoryRepository;
            this.commentRepository = commentRepository;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            ViewBag.SelectedPage = 1;
            var postsCount = (await postRepository.Get(post => post.IsConfirmed == true)).Count();
            var pageCount = (int)Math.Ceiling(postsCount / 20.0);
            ViewBag.PageCount = pageCount;
            var postViewList = await GetPartPosts(i => i.IsConfirmed == true, 0, 20);
            ViewData["Category"] = await categoryRepository.GetAll();

            return View(postViewList);
        }

        [Route("~/Posts/Index/{categoryId:int}")]
        public async Task<IActionResult> Index(int categoryId)
        {
            ViewBag.SelectedPage = 1;
            ViewBag.SelectedCategory = categoryId;
            var postsCount = (await postRepository.Get(post => (post.CategoryId == categoryId) && (post.IsConfirmed == true))).Count();
            var pageCount = (int)Math.Ceiling(postsCount / 20.0);
            var postViewList = await GetPartPosts(post => (post.CategoryId == categoryId) && (post.IsConfirmed == true), 0, 20);
            ViewData["Category"] = await categoryRepository.GetAll();

            return View(postViewList);
        }

        public async Task<IActionResult> IndexPartPosts(int numberPage)
        {
            ViewBag.SelectedPage = numberPage;
            var postsCount = (await postRepository.Get(post => post.IsConfirmed == true)).Count();
            var pageCount = (int)Math.Ceiling(postsCount / 20.0);
            if (pageCount < numberPage || numberPage < 1 )
            {
                return NotFound();
            }
            ViewBag.PageCount = pageCount;
            var postViewList = await GetPartPosts(i => i.IsConfirmed == true, 
                (numberPage - 1) * 20, numberPage != pageCount ? 20 : postsCount - 20 * (numberPage-1));
            ViewData["Category"] = await categoryRepository.GetAll();

            return View("Index", postViewList);
        }

        [Route("~/Posts/IndexPartPosts/{categoryId:int}/{numberPage:int}")]
        public async Task<IActionResult> IndexPartPosts(int categoryId, int numberPage)
        {
            ViewBag.SelectedPage = numberPage;
            ViewBag.SelectedCategory = categoryId;
            var postsCount = (await postRepository.Get(post => (post.CategoryId == categoryId) && (post.IsConfirmed == true))).Count();
            var pageCount = (int)Math.Ceiling(postsCount / 20.0);
            if (pageCount < numberPage || numberPage < 1)
            {
                return NotFound();
            }
            ViewBag.PageCount = pageCount;

            var postViewList = await GetPartPosts(post => (post.CategoryId == categoryId) && (post.IsConfirmed == true),
                (numberPage - 1) * 20, numberPage != pageCount ? 20 : postsCount - 20 * (numberPage - 1));
            ViewData["Category"] = await categoryRepository.GetAll();

            return View("Index", postViewList);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }
            var user = userManager.GetUserAsync(User).Result;
            //Can be done using AuthorizationHandler.
            if (!post.IsConfirmed && user == null)
            {
                return Forbid();
            }
            if (user != null)
            {
                var checkUserRole = await userManager.IsInRoleAsync(user, "Admin");
                if (!post.IsConfirmed && !checkUserRole && user.Id != post.UserId)
                {
                    return Forbid();
                }
            }
            var comments = post.Comments.Where(p => p.PostId == id).ToList();
            comments.Sort(new CommentsComparer());

            ViewData["Comments"] = comments;
            ViewData["Post"] = post;

            return View();
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName");

            return View();
        }

        private byte[] LoadDefaultImage()
        {
            byte[] imageData = null;
            var file = new FileStream($"./wwwroot/images/post.png", FileMode.Open);
            var length = file.Length;

            using (var binaryReader = new BinaryReader(file))
            {
                imageData = binaryReader.ReadBytes((int)length);
            }
            return imageData;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId,HasImage,FormPostImage")] Post post)
        {
            if (!post.HasImage)
            {
                ModelState["FormPostImage"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                post.PostImage = LoadDefaultImage();
            }

            if (ModelState.IsValid)
            {
                if (post.HasImage)
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(post.FormPostImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)post.FormPostImage.Length);
                    }
                    post.PostImage = imageData;
                }

                post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                post.CreationTime = DateTime.Now;
                post.LastModifiedTime = DateTime.Now;
                post.IsConfirmed = false;

                await postRepository.Create(post);

                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);

            return View(post);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }
            //Can be done using AuthorizationHandler.
            if (userManager.GetUserAsync(User).Result.Id != post.UserId)
            {
                return Forbid();
            }
            post.ImageData = Convert.ToBase64String(post.PostImage);
            ViewData["ImageData"] = post.PostImage;
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Title,Content,UserId,CategoryId,CreationTime,ImageData,HasImage,FormPostImage,Id")] Post post)
        {

            if (id != post.Id)
            {
                return NotFound();
            }
            //Can be done using AuthorizationHandler.
            if (userManager.GetUserAsync(User).Result.Id != post.UserId)
            {
                return Forbid();
            }

            if (!post.HasImage)
            {
                ModelState["FormPostImage"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                post.PostImage = Convert.FromBase64String(post.ImageData);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (post.HasImage)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(post.FormPostImage.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)post.FormPostImage.Length);
                        }
                        post.PostImage = imageData;
                        post.HasImage = true;
                    }
                    post.LastModifiedTime = DateTime.Now;
                    post.IsConfirmed = false;

                    await postRepository.Update(post);

                }
                catch (DbUpdateConcurrencyException)
                {
                    var isPostExist = await PostExists(post.Id);
                    if (!isPostExist)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute("default", new { controller = "Posts", action = "Details", id = post.Id });
            }
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);

            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }
            //Can be done using AuthorizationHandler.
            if (userManager.GetUserAsync(User).Result.Id != post.UserId)
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize(Roles = "Admin,User")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await postRepository.Remove(id);
            var commentsListForDelete = new List<Comment>(await commentRepository.Get((commentOfPost) => id == commentOfPost.PostId));
            if (commentsListForDelete.Count != 0)
            {
                foreach (var count in commentsListForDelete)
                {
                    await commentRepository.Remove(count.Id);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("~/Posts/PublishPost/{id:int}")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> PublishPost(int? id)
        {
            var post = await postRepository.FindById(id);
            post.IsConfirmed = true;
            await postRepository.Update(post);

            return Redirect("~/Admin/IndexUnverifiedPosts");
        }

        private async Task<List<PostViewModel>> GetPartPosts(Func<Post, bool> predicate, int skip, int take)
        {
            var allPosts = (await postRepository.Get(predicate)).ToList();
            allPosts.Sort(new PostsComparer());

            var query = allPosts
                        .Skip(skip)
                        .Take(take);
            var postViewQuery = query.Select(p => new PostViewModel()
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                UserName = p.User.UserName,
                Title = p.Title,
                CategoryName = p.Category.CategoryName,
                PostImage = p.PostImage,
                CreationTime = p.CreationTime,
                LastModifiedTime = p.LastModifiedTime

            });
            return postViewQuery.ToList();
        }

        public async Task<ActionResult> GetPosts(int pageIndex, int pageSize)
        {
            return Json(await GetPartPosts(i => i.IsConfirmed == true, pageIndex * pageSize + 3, pageSize));
        }

        private async Task<bool> PostExists(int id)
        {
            return (await postRepository.GetAll()).Any(e => e.Id == id);
        }


    }
}

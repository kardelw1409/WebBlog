using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    public class PostsController : Controller
    {
        private IRepository<Post> postRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<Comment> commentRepository;

        public PostsController(IRepository<Post> postRepository, IRepository<Category> categoryRepository,
            IRepository<Comment> commentRepository)
        {
            this.postRepository = postRepository;
            this.categoryRepository = categoryRepository;
            this.commentRepository = commentRepository;

        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var postList = await postRepository.GetAll();
            return View(postList);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var postAndComments = new PostWithCommentsViewModel();
            var post = await postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }
            postAndComments.Post = post;
            postAndComments.Comments = await commentRepository.Get(p => p.PostId == id);

            return View(postAndComments);
        }

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName");

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId,IsImage,PostImage")] PostViewModel postView)
        {
            var post = new Post { Title = postView.Title, Content = postView.Content, CategoryId = postView.CategoryId, IsImage = postView.IsImage };
            post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            post.CreateTime = DateTime.Now;
            post.LastModifiedTime = DateTime.Now;
            if (!postView.IsImage)
            {
                ModelState["PostImage"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                byte[] imageData = null;
                FileStream file = new FileStream($"./wwwroot/images/post.png", FileMode.Open);
                var length = file.Length;
                using (var binaryReader = new BinaryReader(file))
                {
                    imageData = binaryReader.ReadBytes((int)length);
                }

                post.PostImage = imageData;
            }
            if (ModelState.IsValid)
            {
                if (postView.IsImage)
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(postView.PostImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)postView.PostImage.Length);
                    }
                    post.PostImage = imageData;
                }

                await postRepository.Create(post);
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(postView);
        }

        [Authorize(Roles = "Admin")]
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
            var postView = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                UserId = post.UserId,
                CategoryId = post.CategoryId,
                Content = post.Content,
                CreateTime = post.CreateTime
            };
            ViewData["ImageData"] = post.PostImage;
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(postView);
        }

        // POST: Posts/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Title,Content,UserId,CategoryId,CreateTime,IsImage,PostImage,Id")] PostViewModel postView)
        {
            var post = new Post
            {
                Id = postView.Id,
                Title = postView.Title,
                UserId = postView.UserId,
                Content = postView.Content,
                CategoryId = postView.CategoryId,
                CreateTime = postView.CreateTime,
                IsImage = postView.IsImage
            };
            if (id != post.Id)
            {
                return NotFound();
            }
            post.LastModifiedTime = DateTime.Now;

            if (!postView.IsImage)
            {
                ModelState["PostImage"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                var postImage = (await postRepository.FindById(id)).PostImage;
                post.PostImage = postImage;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (postView.IsImage)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(postView.PostImage.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)postView.PostImage.Length);
                        }
                        post.PostImage = imageData;
                        post.IsImage = true;

                    }
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(postView);
        }

        // GET: Posts/Delete/5
        [Authorize]
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

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        private async Task<bool> PostExists(int id)
        {
            return (await postRepository.GetAll()).Any(e => e.Id == id);
        }


    }
}

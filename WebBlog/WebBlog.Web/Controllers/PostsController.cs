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
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private IRepository<Post> postRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<CommentOfPost> commentOfPostsRepository;
        private IRepository<PostImage> postImageRepository;

        public PostsController(IRepository<Post> postRepository, IRepository<Category> categoryRepository,
            IRepository<CommentOfPost> commentOfPostsRepository, IRepository<PostImage> postImageRepository)
        {
            this.postRepository = postRepository;
            this.categoryRepository = categoryRepository;
            this.commentOfPostsRepository = commentOfPostsRepository;
            this.postImageRepository = postImageRepository;
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

            var post = await postRepository.FindById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId")] Post post)
        {
            post.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            post.CreateTime = DateTime.Now;
            post.UpdateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                var postId = await postRepository.Create(post);
                return RedirectToRoute("default", new { controller = "Posts", action = "IndexUpload", id = postId });
            }
            ViewBag["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
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
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Content,CategoryId,Id")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            post.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            post.UpdateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
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
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await postRepository.Remove(id);
            var commentsListForDelete = new List<CommentOfPost>(await commentOfPostsRepository.Get(( commentOfPost) => id == commentOfPost.PostId));
            if (commentsListForDelete.Count != 0)
            {
                foreach (var count in commentsListForDelete)
                {
                    await commentOfPostsRepository.Remove(count.Id);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts
        public async Task<IActionResult> IndexUpload(int id)
        {
            var post = await postRepository.FindById(id);
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(Post post, [FromForm]IFormFile uploadedImage)
        {
            if (uploadedImage == null)
            {
                //TO DO
                return RedirectToAction("Index");
            }
            if (uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream memoryStream = new MemoryStream();
                await uploadedImage.OpenReadStream().CopyToAsync(memoryStream);
                var image = Image.FromStream(memoryStream);
                var imageEntity = new PostImage()
                {
                    Name = uploadedImage.Name,
                    Data = memoryStream.ToArray(),
                    Width = image.Width,
                    Height = image.Height,
                    ContentType = uploadedImage.ContentType
                };
                var imageId = await postImageRepository.Create(imageEntity);
                post.PostImageId = imageId;
                await postRepository.Update(post);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewImage(int? id)
        {
            var image = await postImageRepository.FindById(id);
            var ms = new MemoryStream(image.Data);

            return new FileStreamResult(ms, image.ContentType);
        }

        private async Task<bool> PostExists(int id)
        {
            return (await postRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var postList = await postRepository.Get(i => i.IsConfirmed == true);

            ViewData["Category"] = await categoryRepository.GetAll();
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

        [Route("~/Posts/Index/{categoryId:int}")]
        public async Task<IActionResult> Index(int categoryId)
        {
            var postList = await postRepository.Get(post => (post.CategoryId == categoryId) && (post.IsConfirmed == true));
            ViewData["Category"] = await categoryRepository.GetAll();
            var postViewList = postList.Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                PostImage = p.PostImage,
                CategoryName = p.Category.CategoryName,
                CreationTime = p.CreationTime,
                LastModifiedTime = p.LastModifiedTime,
                UserName = p.User.UserName
            });

            return View(postViewList);
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
            ViewData["Post"] = post; 

            ViewData["Comments"] = post.Comments.Where(p => p.PostId == id).ToList();

            return View();
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
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId,HasImage,FormPostImage")] Post post)
        {
            post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            post.CreationTime = DateTime.Now;
            post.LastModifiedTime = DateTime.Now;
            post.IsConfirmed = false;
            if (!post.HasImage)
            {
                ModelState["FormPostImage"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
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
                if (post.HasImage)
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(post.FormPostImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)post.FormPostImage.Length);
                    }
                    post.PostImage = imageData;
                }

                await postRepository.Create(post);
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(await categoryRepository.GetAll(), "Id", "CategoryName", post.CategoryId);
            return View(post);
        }

        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Title,Content,UserId,CategoryId,CreationTime,ImageData,HasImage,FormPostImage,Id")] Post post)
        {

            if (id != post.Id)
            {
                return NotFound();
            }
            if (userManager.GetUserAsync(User).Result.Id != post.UserId)
            {
                return Forbid();
            }
            post.LastModifiedTime = DateTime.Now;
            post.IsConfirmed = false;
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
            if (userManager.GetUserAsync(User).Result.Id != post.UserId)
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
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

        public async Task<ActionResult> GetPosts(int pageIndex, int pageSize)
        {
            var allConfirmedPosts = await postRepository.Get(i => i.IsConfirmed == true);
            var length = allConfirmedPosts.Count();
            if (pageIndex <= (length/pageSize) )
            {
                var query = (from c in allConfirmedPosts
                             orderby c.Title ascending
                             select c)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize);
                var postViewQuery = query.Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    PostImage = p.PostImage,
                    LastModifiedTime = p.LastModifiedTime,
                    UserName = p.User.UserName

                });
                return Json(postViewQuery.ToList());
            }
            return StatusCode(204);
        }

        private async Task<bool> PostExists(int id)
        {
            return (await postRepository.GetAll()).Any(e => e.Id == id);
        }


    }
}

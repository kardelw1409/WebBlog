using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;

namespace WebBlog.Web.Controllers
{
    [Authorize]
    public class CommentOfPostsController : Controller
    {
        private IRepository<Post> postRepository;
        private IRepository<CommentOfPost> commentOfPostRepository;

        public CommentOfPostsController(IRepository<Post> postRepository, IRepository<CommentOfPost> commentOfPostRepository)
        {
            this.postRepository = postRepository;
            this.commentOfPostRepository = commentOfPostRepository;
        }

        // GET: CommentOfPosts
        [Route("~/CommentOfPosts/Index/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var post = await postRepository.FindById(id);
            var listComments = post.CommentsOfPost;
            ViewBag.PostId = id;
            return View(listComments);
        }

        // GET: CommentOfPosts/Create//PostId
        [Route("~/CommentOfPosts/Create/{id:int}")]
        public IActionResult Create(int id)
        {
            ViewBag.PostId = id;
            return View();
        }

        // POST: CommentOfPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/CommentOfPosts/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,PostId")] CommentOfPost commentOfPost)
        {
            commentOfPost.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            commentOfPost.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await commentOfPostRepository.Create(commentOfPost);
                return RedirectToRoute("default", new { controller = "CommentOfPosts", action = "Index",  id = commentOfPost.PostId } );
            }
            return View(commentOfPost);
        }

        // GET: CommentOfPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await commentOfPostRepository.FindById(id);
            if (commentOfPost == null)
            {
                return NotFound();
            }
            return View(commentOfPost);
        }

        // POST: CommentOfPosts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentOfPost = await commentOfPostRepository.FindById(id);
            await commentOfPostRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "CommentOfPosts", action = "Index", id = commentOfPost.PostId });
        }

        private async Task<bool> PostExists(int id)
        {
            return (await commentOfPostRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;

namespace WebBlog.Web.Controllers
{
    public class CommentToCommentsController : Controller
    {
        private IRepository<CommentOfPost> commentOfPostRepository;
        private IRepository<CommentToComment> commentToCommentRepository;

        public CommentToCommentsController(IRepository<CommentOfPost> commentOfPostRepository, IRepository<CommentToComment> commentToCommentRepository)
        {
            this.commentOfPostRepository = commentOfPostRepository;
            this.commentToCommentRepository = commentToCommentRepository;
        }

        // GET: CommentOfPosts
        [Route("~/CommentToComments/Index/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var commentOfPost = await commentOfPostRepository.FindById(id);
            var listComments = commentOfPost.CommentToComments;
            ViewBag.CommentOfPostId = id;
            ViewBag.PostId = commentOfPost.PostId;
            return View(listComments);
        }

        // GET: CommentOfPosts/Create//PostId
        [Route("~/CommentToComments/Create/{id:int}")]
        public IActionResult Create(int? id)
        {
            ViewBag.CommentOfPostId = id;
            return View();
        }

        // POST: CommentOfPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/CommentToComments/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,CommentOfPostId")] CommentToComment commentToComment)
        {
            commentToComment.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            commentToComment.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await commentToCommentRepository.Create(commentToComment);
                return RedirectToRoute("default", new { controller = "CommentToComments", action = "Index", id = commentToComment.CommentOfPostId });
            }
            return View(commentToComment);
        }

        // GET: CommentOfPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await commentToCommentRepository.FindById(id);
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
            var commentOfPost = await commentToCommentRepository.FindById(id);
            await commentToCommentRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "CommentToComments", action = "Index", id = commentOfPost.CommentOfPostId });
        }

        private async Task<bool> PostExists(int id)
        {
            return (await commentToCommentRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}
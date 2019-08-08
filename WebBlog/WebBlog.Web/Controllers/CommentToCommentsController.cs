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
        private IRepository<Comment> commentRepository;

        public CommentToCommentsController(IRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        // GET: CommentOfPosts
        [Route("~/CommentToComments/Index/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var commentOfPost = await commentRepository.FindById(id);
            var listComments = commentOfPost.Children;
            ViewBag.CommentOfPostId = id;
            ViewBag.PostId = commentOfPost.PostId;
            return View(listComments);
        }

        // GET: CommentOfPosts/Create/id
        [Route("~/CommentToComments/Create/{id:int}")]
        public IActionResult Create(int? id)
        {
            ViewBag.ParentCommentId = id;
            return View();
        }

        // POST: CommentOfPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/CommentToComments/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,ParentCommentId")] Comment comment)
        {
            var parentComment = await commentRepository.FindById(comment.ParentCommentId);
            comment.PostId = parentComment.PostId;
            comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            comment.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await commentRepository.Create(comment);
                return RedirectToRoute("default", new { controller = "CommentToComments", action = "Index", id = comment.ParentCommentId });
            }
            return View(comment);
        }

        // GET: CommentOfPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await commentRepository.FindById(id);
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
            var commentOfPost = await commentRepository.FindById(id);
            await commentRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "CommentToComments", action = "Index", id = commentOfPost.ParentCommentId });
        }

        private async Task<bool> PostExists(int id)
        {
            return (await commentRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}
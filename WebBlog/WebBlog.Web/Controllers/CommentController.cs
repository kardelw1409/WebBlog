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
    [Authorize(Roles = "Admin,User")]
    public class CommentController : Controller
    {
        IUnitOfWork unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Comment
        [Route("~/Comment/Index/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var post = await unitOfWork.PostRepository.FindById(id);
            var listComments = post.Comments.Where(parent => parent.ParentId == null);
            ViewBag.PostId = id;
            return View(listComments);
        }

        // GET: Comment/Create/PostId
        [Route("~/Comment/Create/{id:int}")]
        public IActionResult Create(int id)
        {
            ViewBag.PostId = id;
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Comment/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,PostId")] Comment comment)
        {
            comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            comment.CreationTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await unitOfWork.CommentRepository.Create(comment);
                return RedirectToRoute("default", new { controller = "Posts", action = "Details",  id = comment.PostId } );
            }
            return View(comment);
        }



        // GET: Comment/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await unitOfWork.CommentRepository.FindById(id);
            if (commentOfPost == null)
            {
                return NotFound();
            }
            return View(commentOfPost);
        }

        // POST: Comment/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentOfPost = await unitOfWork.CommentRepository.FindById(id);
            var commentIdForDelete = new List<int>();
            foreach (var child in commentOfPost.Children)
            {
                commentIdForDelete.Add(child.Id);
            }
            foreach (var count in commentIdForDelete)
            {
                await unitOfWork.CommentRepository.Remove(count);
            }
            await unitOfWork.CommentRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "Comment", action = "Index", id = commentOfPost.PostId });
        }



        [Route("~/Comment/Index/{postId:int}/{commentId:int}")]
        public async Task<IActionResult> Index(int postId, int commentId)
        {
            var commentOfPost = await unitOfWork.CommentRepository.FindById(commentId);
            var listComments = commentOfPost.Children;
            ViewBag.ParentId = commentId;
            ViewBag.PostId = postId;
            return View(listComments);
        }


        [Route("~/Comment/Create/{postId:int}/{commentId:int}")]
        public IActionResult Create(int postId, int commentId)
        {
            ViewBag.PostId = postId;
            ViewBag.ParentId = commentId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Comment/Create/{postId:int}/{commentId:int}")]
        public async Task<IActionResult> Create(int postId, int commentId, [Bind("Content,ParentId")] Comment comment)
        {
            var parentComment = await unitOfWork.CommentRepository.FindById(commentId);
            comment.PostId = postId;
            comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            comment.CreationTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await unitOfWork.CommentRepository.Create(comment);
                return RedirectToRoute("default", new { controller = "Posts", action = "Details", id = comment.PostId });
            }
            return View(comment);
        }


        private async Task<bool> PostExists(int id)
        {
            return (await unitOfWork.CommentRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}

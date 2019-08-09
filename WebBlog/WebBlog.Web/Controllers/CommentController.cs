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
    public class CommentController : Controller
    {
        private IRepository<Post> postRepository;
        private IRepository<Comment> commentRepository;

        public CommentController(IRepository<Post> postRepository, IRepository<Comment> commentRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        // GET: CommentOfPosts
        [Route("~/Comment/Index/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var post = await postRepository.FindById(id);
            var listComments = post.CommentsOfPost.Where(parent => parent.ParentCommentId == null);
            ViewBag.PostId = id;
            return View(listComments);
        }

        // GET: CommentOfPosts/Create/PostId
        [Route("~/Comment/Create/{id:int}")]
        public IActionResult Create(int id)
        {
            ViewBag.PostId = id;
            return View();
        }

        // POST: CommentOfPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/Comment/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,PostId")] Comment comment)
        {
            comment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            comment.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await commentRepository.Create(comment);
                return RedirectToRoute("default", new { controller = "Posts", action = "Details",  id = comment.PostId } );
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
            var commentIdForDelete = new List<int>();
            foreach (var child in commentOfPost.Children)
            {
                commentIdForDelete.Add(child.Id);
            }
            foreach (var count in commentIdForDelete)
            {
                await commentRepository.Remove(count);
            }
            await commentRepository.Remove(id);
            return RedirectToRoute("default", new { controller = "Comment", action = "Index", id = commentOfPost.PostId });
        }
        /*
        // GET: CommentOfPosts
        [Route("~/CommentToComments/Index/{postId:int}/{id:int}")]
        public async Task<IActionResult> Index(int postId, int id)
        {
            var commentOfPost = await commentRepository.FindById(id);
            var listComments = commentOfPost.Children;
            ViewBag.CommentOfPostId = id;
            ViewBag.PostId = postId;
            return View(listComments);
        }

        // GET: CommentOfPosts/Create/id
        [Route("~/CommentToComments/Create/{id:int}")]
        public IActionResult Create(int? id)
        {
            ViewBag.ParentCommentId = id;
            return View();
        }

        // POST: CommentToComment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/CommentToComments/Create/{id:int}")]
        public async Task<IActionResult> Create([Bind("Content,ParentCommentId")] Comments comment)
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
        }*/

        private async Task<bool> PostExists(int id)
        {
            return (await commentRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}

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
using WebBlog.ApplicationCore.Interfaces;

namespace WebBlog.Web.Controllers
{
    [Authorize]
    public class CommentOfPostsController : Controller
    {
        private IFullRepository<Post> postRepository;
        private IMainRepository<CommentOfPost> commentOfPostRepository;

        public CommentOfPostsController(IFullRepository<Post> postRepository, IMainRepository<CommentOfPost> commentOfPostRepository)
        {
            this.postRepository = postRepository;
            this.commentOfPostRepository = commentOfPostRepository;
        }

        // GET: CommentOfPosts
        [Route("CommentOfPosts/Index/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            var post = await postRepository.FindById(id);
            var listComments = post.CommentOfPosts;
            return View(listComments);
        }

        /*// GET: CommentOfPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await _context.CommentOfPosts
                .Include(c => c.ApplicationUser)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentOfPost == null)
            {
                return NotFound();
            }

            return View(commentOfPost);
        }*/
        // GET: CommentOfPosts/Create
        public IActionResult Create(int? id)
        {
            ViewData["PostId"] = id;
            return View();
        }

        // POST: CommentOfPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,PostId")] CommentOfPost commentOfPost)
        {
            commentOfPost.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            commentOfPost.CreateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                await commentOfPostRepository.Create(commentOfPost);
                return RedirectToAction(nameof(Create));
            }
            return View(commentOfPost);
        }

        // GET: CommentOfPosts/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentOfPost = await _context.CommentOfPosts.FindAsync(id);
            if (commentOfPost == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", commentOfPost.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", commentOfPost.PostId);
            return View(commentOfPost);
        }

        // POST: CommentOfPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Content,CreateTime,ApplicationUserId,Id")] CommentOfPost commentOfPost)
        {
            if (id != commentOfPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentOfPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentOfPostExists(commentOfPost.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", commentOfPost.ApplicationUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", commentOfPost.PostId);
            return View(commentOfPost);
        }*/
        [Authorize(Roles = "Admin")]
        // GET: CommentOfPosts/Delete/5
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
        [Authorize(Roles = "Admin")]
        // POST: CommentOfPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await commentOfPostRepository.Remove(id);
            return RedirectToAction("/Post/Index");
        }

        private async Task<bool> PostExists(int id)
        {
            return (await commentOfPostRepository.GetAll()).Any(e => e.Id == id);
        }
    }
}

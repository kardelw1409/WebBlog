using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Interfaces;
using WebBlog.ApplicationCore.Repositories;

namespace WebBlog.Web.Controllers
{
    public class AuthorController : Controller
    {
        private IFullRepository<Author> authorRepository;

        public AuthorController(IFullRepository<Author> authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        // POST: /Author/Create

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
           [Bind(Include = "Id, Nickname, Email, FirstName, LastName, Created, PriorityCategoryId, Category, AvatarImageId, AvatarImage, Posts")]
           Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    authorRepository.Create(author);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {

                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(author);
        }*/

        public IActionResult Index()
        {
            var list = authorRepository.GetAll();
            return View(list);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private IRepository<Category> categoryRepository;

        public CategoriesController(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await categoryRepository.GetAll());
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,Description,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                await categoryRepository.Create(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.FindById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await categoryRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

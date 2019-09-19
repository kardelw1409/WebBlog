using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;

namespace WebBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await unitOfWork.CategoryRepository.GetAll());
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
                await unitOfWork.CategoryRepository.Create(category);
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

            var category = await unitOfWork.CategoryRepository.FindById(id);
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
            var category = await unitOfWork.CategoryRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

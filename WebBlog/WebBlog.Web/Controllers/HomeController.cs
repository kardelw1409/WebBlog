using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Infrastructures;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        UserManager<ApplicationUser> userManager;
        IUnitOfWork unitOfWork;
        ISomeServiceRepository<Weather> serviceRepository;
        public HomeController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ISomeServiceRepository<Weather> serviceRepository)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.serviceRepository = serviceRepository;
        }
        public async Task<IActionResult> Index()
        {
            var postList = (await unitOfWork.PostRepository.Get(i => i.IsConfirmed == true)).ToList();
            postList.Sort(new PostsComparer());

            var newList = postList.Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                LastModifiedTime = p.LastModifiedTime,
                PostImage = p.PostImage,
                UserName = p.User.UserName
            });
            try
            {
                ViewBag.Weather = await serviceRepository.GetData();
            }
            catch
            {
                ViewBag.Weather = new Weather()
                {
                    CityName = "City not found"
                };
            }
            return View(newList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Web Blog description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> RolesMissing()
        {
            var user = (await userManager.GetUserAsync(User));
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Where(p => p == "User" || p == "Admin").Count() != 0)
            {
                return Forbid();
            }
            return View();
        }
    }
}

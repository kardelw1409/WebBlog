using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.Web.Models;

namespace WebBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Post> postRepository;
        public HomeController(IRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }
        public async Task<IActionResult> Index()
        {
            var postList = (List<Post>)await postRepository.GetAll();
            if (postList.Count > 5)
            {
                postList = (from t in postList
                            orderby t.CreationTime
                            select t).Take(5).ToList();
            }

            return View(postList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
    }
}

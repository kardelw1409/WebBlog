﻿using System;
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
            var postList = (await postRepository.Get(i => i.IsConfirmed == true)).ToList();
            /*if (postList.Count > 5)
            {
                postList = (from t in postList
                            orderby t.CreationTime
                            select t).Take(5).ToList();
            }*/
            return View(postList);
        }

        public async Task<ActionResult> GetData(int pageIndex, int pageSize)
        {

            var query = (from c in await postRepository.Get(i => i.IsConfirmed == true)
                         orderby c.Title ascending
                         select c)
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize);
            return Json(query.ToList());
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

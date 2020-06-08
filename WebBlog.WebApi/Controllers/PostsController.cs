using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.WebApi.Core.JsonModels;
using WebBlog.WebApi.Core.Services;

namespace WebBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private PostService _postService;

        public PostsController(IUnitOfWork unitOfWork)
        {
            _postService = new PostService(unitOfWork.PostRepository);
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<IEnumerable<PostModel>> Get()
        {
            return await _postService.GetPostHomeModels(p => p.IsConfirmed);
        }

        // GET: api/Posts/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<PostModel> Get(int id)
        {
            return await _postService.GetPost(id);
        }

        // POST: api/Posts
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

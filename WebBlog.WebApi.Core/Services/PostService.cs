using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;
using WebBlog.WebApi.Core.JsonModels;

namespace WebBlog.WebApi.Core.Services
{
    public class PostService
    {
        private IRepository<Post> _postRepository;

        public PostService(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostModel>> GetPostHomeModels(Func<Post, bool> predicate, int skip = 0, int take = 20)
        {
            var posts = (await _postRepository.Get(predicate)).Skip(skip).Take(take).ToList();

            return posts.Select(p => new PostModel
            {
                Id = p.Id,
                LastModifiedTime = p.LastModifiedTime,
                PostImage = p.PostImage,
                Title = p.Title,
                UserName = p.User.UserName
            }).OrderBy(p => p.LastModifiedTime);
        }

        public async Task<IEnumerable<PostModel>> GetPostModels(Func<Post, bool> predicate, int skip = 0, int take = 20)
        {
            var posts = (await _postRepository.Get(predicate)).Skip(skip).Take(take).ToList();

            return posts.Select(p => new PostModel()
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                UserName = p.User.UserName,
                Title = p.Title,
                CategoryName = p.Category?.CategoryName,
                PostImage = p.PostImage,
                CreationTime = p.CreationTime,
                LastModifiedTime = p.LastModifiedTime

            }).OrderBy(p => p.LastModifiedTime);
        }

        public async Task<PostModel> GetPost(int id)
        {
            var post = await _postRepository.FindById(id);

            return post != null ? new PostModel
            {
                Id = post.Id,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.CategoryName,
                CreationTime = post.CreationTime,
                LastModifiedTime = post.LastModifiedTime,
                PostImage = post.PostImage,
                Title = post.Title,
                UserName = post.User.UserName,
                UserId = post.UserId
            } : null;
        }
    }
}
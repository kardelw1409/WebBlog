using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<IEnumerable<PostHomeModel>> GetAll()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Post, PostHomeModel>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<Post>, IEnumerable<PostHomeModel>>( await _postRepository.GetAll());
        }
    }
}
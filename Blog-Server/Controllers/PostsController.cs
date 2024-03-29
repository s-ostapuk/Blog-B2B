using Blog_Server.Interfaces.Services;
using Blog_Server.Models.ResponseModels.BlogResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _blogService;

        public PostsController(IPostsService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<GetAllPostsResponseModel> GetAllPosts()
        {
            return await _blogService.GetAllPostsAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<GetPostByIdResponseModel> GetPostById([FromRoute] int id)
        {
            return await _blogService.GetPostByIdAsync(id);
        }

    }
}

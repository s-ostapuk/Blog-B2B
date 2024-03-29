using Blog_Server.Interfaces.Services;
using Blog_Server.Models.DtoModels;
using Blog_Server.Models.RequestModels;
using Blog_Server.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        #region PostsEndpoints
        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetAllPosts()
        {
            return await _blogService.GetAllPostsAsync();
        }
        
        [HttpGet]
        [Route("{postId:maxlength(9)}")]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetPostById([FromRoute] int postId)
        {
            return await _blogService.GetPostByIdAsync(postId);
        }
        
        [HttpPost]
        [Route("create")]
        public async Task<BaseResponseModel> CreateNewPost([FromBody] CreateNewPostRequestModel requestModel)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.CreateNewPostAsync(requestModel, username);
        }
        
        [HttpPut]
        [Route("{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> UpdatePost([FromRoute] int postId, [FromBody] UpdatePostRequestModel requestModel)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.UpdatePostAsync(requestModel, postId, username);
        }
        
        [HttpDelete]
        [Route("{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> DeletePost([FromRoute] int postId)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.DeletePostAsync(postId, username);
        }
        #endregion
        #region CommentsEndpoints
        [HttpGet]
        [Route("{postId:maxlength(9)}/comments")]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetCommentsByPostId([FromRoute] int postId)
        {
            return await _blogService.GetCommentsByPostIdAsync(postId);
        }

        [HttpPost]
        [Route("{postId:maxlength(9)}/comments/create")]
        public async Task<BaseResponseModel> CreateNewPostComment([FromRoute] int postId, [FromBody] CreateNewPostCommentRequestModel requestModel)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.CreateNewPostCommentAsync(requestModel, postId, username);
        }

        [HttpPut]
        [Route("{postId:maxlength(9)}/comments/{commentId:maxlength(6)}")]
        public async Task<BaseResponseModel> UpdatePostComment([FromRoute] int postId, [FromRoute] int commentId, [FromBody] UpdatePostCommentRequestModel requestModel)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.UpdatePostCommentAsync(requestModel, postId, commentId, username);
        }

        [HttpDelete]
        [Route("{postId:maxlength(9)}/comments/{commentId:maxlength(6)}")]
        public async Task<BaseResponseModel> DeletePostComment([FromRoute] int postId, [FromRoute] int commentId)
        {
            var username = await GetUserLoginFromToken();
            return await _blogService.DeletePostCommentAsync(postId, commentId, username);
        }
        #endregion

        private async Task<string?> GetUserLoginFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

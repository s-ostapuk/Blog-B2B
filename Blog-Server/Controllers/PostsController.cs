using Blog_Server.Exceptions;
using Blog_Server.Interfaces.Services;
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
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;

        public PostsController(IPostsService postsService, ICommentsService commentsService)
        {
            _postsService = postsService;
            _commentsService = commentsService;
        }
        #region PostsEndpoints
        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetAllPosts()
        {
            return await _postsService.GetAllPostsAsync();
        }
        
        [HttpGet]
        [Route("{postId:maxlength(9)}")]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetPostById([FromRoute] int postId)
        {
            return await _postsService.GetPostByIdAsync(postId);
        }
        
        [HttpPost]
        [Route("create")]
        public async Task<BaseResponseModel> CreateNewPost([FromBody] CreateNewPostRequestModel requestModel)
        {
            return await _postsService.CreateNewPostAsync(requestModel, GetUserLoginFromToken());
        }
        
        [HttpPut]
        [Route("{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> UpdatePost([FromRoute] int postId, [FromBody] UpdatePostRequestModel requestModel)
        {
            return await _postsService.UpdatePostAsync(requestModel, postId, GetUserLoginFromToken());
        }
        
        [HttpDelete]
        [Route("{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> DeletePost([FromRoute] int postId)
        {
            return await _postsService.DeletePostAsync(postId, GetUserLoginFromToken());
        }
        #endregion
        #region CommentsEndpoints
        [HttpGet]
        [Route("{postId:maxlength(9)}/comments")]
        [AllowAnonymous]
        public async Task<BaseResponseModel> GetCommentsByPostId([FromRoute] int postId)
        {
            return await _commentsService.GetCommentsByPostIdAsync(postId);
        }

        [HttpPost]
        [Route("{postId:maxlength(9)}/comments/create")]
        public async Task<BaseResponseModel> CreateNewPostComment([FromRoute] int postId, [FromBody] CreateNewPostCommentRequestModel requestModel)
        {
            return await _commentsService.CreateNewPostCommentAsync(requestModel, postId, GetUserLoginFromToken());
        }

        [HttpPut]
        [Route("{postId:maxlength(9)}/comments/{commentId:maxlength(6)}")]
        public async Task<BaseResponseModel> UpdatePostComment([FromRoute] int postId, [FromRoute] int commentId, [FromBody] UpdatePostCommentRequestModel requestModel)
        {
            return await _commentsService.UpdatePostCommentAsync(requestModel, postId, commentId, GetUserLoginFromToken());
        }

        [HttpDelete]
        [Route("{postId:maxlength(9)}/comments/{commentId:maxlength(6)}")]
        public async Task<BaseResponseModel> DeletePostComment([FromRoute] int postId, [FromRoute] int commentId)
        {
            return await _commentsService.DeletePostCommentAsync(postId, commentId, GetUserLoginFromToken());
        }
        #endregion

        private string GetUserLoginFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new AppException("Required claim doesn`t exist");
        }
    }
}

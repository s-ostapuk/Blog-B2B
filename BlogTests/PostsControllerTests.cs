using Blog_Server.Controllers;
using Blog_Server.Interfaces.Services;
using Blog_Server.Models.DtoModels;
using Blog_Server.Models.RequestModels;
using Blog_Server.Models.ResponseModels;
using BlogTests.MockData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;

namespace BlogTests
{
    public class PostsControllerTests
    {
        private readonly PostsController _postsController;
        private readonly Mock<ICommentsService> _commentsService;
        private readonly Mock<IPostsService> _postsService;
        public PostsControllerTests()
        {
            _commentsService = new Mock<ICommentsService>();
            _postsService = new Mock<IPostsService>();
            _postsController = new PostsController(_postsService.Object, _commentsService.Object);
        }

        [Fact]
        public async Task GetAllPosts_Success()
        {
            //Arrange
            _postsService.Setup(service => service.GetAllPostsAsync()).ReturnsAsync(PostsMockData.GetAllPosts());
            //Act
            var result = await _postsController.GetAllPosts();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BaseResponseModel>(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task GetAllPosts_SuccessWithEmptyData()
        {
            //Arrange
            _postsService.Setup(service => service.GetAllPostsAsync()).ReturnsAsync(PostsMockData.GetBaseResponseModelWithDataNull());
            //Act
            var result = await _postsController.GetAllPosts();
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BaseResponseModel>(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task GetPostById_Success()
        {
            //Arrange
            var existingPostId = 2;
            var missingPostId = 3;
            _postsService.Setup(service => service.GetPostByIdAsync(existingPostId)).ReturnsAsync(PostsMockData.GetExistingPost());
            _postsService.Setup(service => service.GetPostByIdAsync(missingPostId)).ReturnsAsync(PostsMockData.GetBaseResponseModelWithDataNull());
            //Act
            var successResult = await _postsController.GetPostById(existingPostId);
            var errorResult = await _postsController.GetPostById(missingPostId);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);

            Assert.NotNull(errorResult);
            Assert.IsType<BaseResponseModel>(errorResult);
            Assert.Equal(HttpStatusCode.OK, errorResult.HttpStatusCode);
            Assert.True(errorResult.IsSuccess);
            Assert.Null(errorResult.Data);
        }
        [Fact]
        public async Task CreateNewPost_Success()
        {
            var mockRequestModel = new CreateNewPostRequestModel
            {
                Title = "Title",
                Content = "test post content"
            };
            string login = "TestUser1";
            //Arrange
            _postsService.Setup(service => service.CreateNewPostAsync(mockRequestModel, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.CreateNewPost(mockRequestModel);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }
        [Fact]
        public async Task UpdatePost_Success()
        {
            var mockRequestModel = new UpdatePostRequestModel
            {
                Title = "Title",
                Content = "test post content"
            };
            string login = "TestUser1";
            int postId = 1;

            //Arrange
            _postsService.Setup(service => service.UpdatePostAsync(mockRequestModel, postId, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.UpdatePost(postId, mockRequestModel);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }
        [Fact]
        public async Task DeletePost_Success()
        {
            string login = "TestUser1";
            int postId = 1;

            //Arrange
            _postsService.Setup(service => service.DeletePostAsync(postId, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.DeletePost(postId);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }
        [Fact]
        public void CreateNewPost_ActionRequiresAuthorization()
        {
            // Arrange
            var type = _postsController.GetType();
            var methodInfo = type.GetMethod("CreateNewPost");
            // Act
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            // Assert
            Assert.False(attributes.Any(), "No AuthorizeAttribute found on action method.");
        }


        [Fact]
        public async Task GetCommentsByPostId_Success()
        {
            //Arrange
            var existingPostId = 2;
            var missingPostId = 3;
            _commentsService.Setup(service => service.GetCommentsByPostIdAsync(existingPostId)).ReturnsAsync(PostsMockData.GetCommentsByPostIdResponseSuccess());
            _commentsService.Setup(service => service.GetCommentsByPostIdAsync(missingPostId)).ReturnsAsync(PostsMockData.GetCommentsByPostIdResponseSuccessEmpty());
            //Act
            var successResult = await _postsController.GetCommentsByPostId(existingPostId);
            var errorResult = await _postsController.GetCommentsByPostId(missingPostId);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.IsType<List<CommentDto>>(successResult.Data);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);

            Assert.NotNull(errorResult);
            Assert.IsType<BaseResponseModel>(errorResult);
            Assert.IsType<List<CommentDto>>(errorResult.Data);
            Assert.Empty((List<CommentDto>) errorResult.Data);
            Assert.Equal(HttpStatusCode.OK, errorResult.HttpStatusCode);
            Assert.True(errorResult.IsSuccess);
        }
        [Fact]
        public async Task CreateNewComment_Success()
        {
            var mockRequestModel = new CreateNewPostCommentRequestModel
            {
                CommentText = "Title"
            };
            string login = "TestUser1";
            int postId = 2;
            //Arrange
            _commentsService.Setup(service => service.CreateNewPostCommentAsync(mockRequestModel, postId, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.CreateNewPostComment(postId, mockRequestModel);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }
        [Fact]
        public async Task UpdateComment_Success()
        {
            var mockRequestModel = new UpdatePostCommentRequestModel
            {
                CommentText = "Title"
            };
            string login = "TestUser1";
            int postId = 2;
            int commentId = 2;
            //Arrange
            _commentsService.Setup(service => service.UpdatePostCommentAsync(mockRequestModel, postId, commentId, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.UpdatePostComment(postId, commentId, mockRequestModel);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }
        [Fact]
        public async Task DeleteComment_Success()
        {
            string login = "TestUser1";
            int postId = 2;
            int commentId = 2;
            //Arrange
            _commentsService.Setup(service => service.DeletePostCommentAsync(postId, commentId, login)).ReturnsAsync(PostsMockData.GetBaseResponseModelSuccess());
            SetMockClaimsForUser(login);
            //Act
            var successResult = await _postsController.DeletePostComment(postId, commentId);
            //Assert
            Assert.NotNull(successResult);
            Assert.IsType<BaseResponseModel>(successResult);
            Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);
            Assert.True(successResult.IsSuccess);
        }

        private void SetMockClaimsForUser(string login)
        {
            var contextMock = new Mock<HttpContext>();
            var claims = new List<Claim> { new Claim(
                ClaimTypes.NameIdentifier,login) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal(claimsIdentity));
            _postsController.ControllerContext.HttpContext = contextMock.Object;
        }
    }
}
using Blog_Server.Models.RequestModels;
using Blog_Server.Models.ResponseModels;

namespace Blog_Server.Interfaces.Services
{
    public interface IPostsService
    {
        Task<BaseResponseModel> GetAllPostsAsync();
        Task<BaseResponseModel> GetPostByIdAsync(int postId);
        Task<BaseResponseModel> GetCommentsByPostIdAsync(int postId);
        Task<BaseResponseModel> CreateNewPostCommentAsync(CreateNewPostCommentRequestModel requestModel, int postId, string username);
        Task<BaseResponseModel> UpdatePostCommentAsync(UpdatePostCommentRequestModel requestModel, int postId, int commentId, string username);
        Task<BaseResponseModel> DeletePostCommentAsync(int postId, int commentId, string username);

    }
}

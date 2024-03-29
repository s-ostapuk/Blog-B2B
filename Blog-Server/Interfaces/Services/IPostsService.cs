using Blog_Server.Models.ResponseModels.BlogResponseModels;

namespace Blog_Server.Interfaces.Services
{
    public interface IPostsService
    {
        Task<GetAllPostsResponseModel> GetAllPostsAsync();
        Task<GetPostByIdResponseModel> GetPostByIdAsync(int postId);
    }
}

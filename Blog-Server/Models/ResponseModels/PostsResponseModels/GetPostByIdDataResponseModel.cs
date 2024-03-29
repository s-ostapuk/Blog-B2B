using Blog_Server.Models.DtoModels;

namespace Blog_Server.Models.ResponseModels.PostsResponseModels
{
    public class GetPostByIdDataResponseModel
    {
        public BlogPostDto? Post { get; set; } = null;
    }
}

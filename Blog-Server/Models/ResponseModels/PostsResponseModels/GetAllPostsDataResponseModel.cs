using Blog_Server.Models.DtoModels;

namespace Blog_Server.Models.ResponseModels.PostsResponseModels
{
    public class GetAllPostsDataResponseModel
    {
        public List<BlogPostDto> blogPosts { get; set; } = new List<BlogPostDto>();
    }
}

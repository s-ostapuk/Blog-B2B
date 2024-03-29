using Blog_Server.Models.DtoModels;

namespace Blog_Server.Models.ResponseModels.BlogResponseModels
{
    public class GetAllPostsResponseModel : BaseResponseModel
    {
        public List<BlogPostDto> blogPosts { get; set; } = new List<BlogPostDto>();
    }
}

using Blog_Server.Models.DtoModels;

namespace Blog_Server.Models.ResponseModels.BlogResponseModels
{
    public class GetPostByIdResponseModel : BaseResponseModel
    {
        public BlogPostDto? post { get; set; }
    }
}

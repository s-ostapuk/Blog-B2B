using Blog_Server.Models.DtoModels;

namespace Blog_Server.Models.ResponseModels.PostsResponseModels
{
    public class GetCommentsByPostIdModel
    {
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}

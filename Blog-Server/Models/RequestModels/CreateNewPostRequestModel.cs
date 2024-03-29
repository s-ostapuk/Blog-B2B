namespace Blog_Server.Models.RequestModels
{
    public class CreateNewPostRequestModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}

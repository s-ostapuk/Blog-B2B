using AutoMapper;
using Blog_Server.Interfaces.Services;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Models.DtoModels;
using Blog_Server.Models.ResponseModels.BlogResponseModels;

namespace Blog_Server.Services
{
    public class PostsService : IPostsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PostsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllPostsResponseModel> GetAllPostsAsync()
        {
            return new GetAllPostsResponseModel()
            {
                blogPosts = _mapper.Map<List<BlogPostDto>>(await _unitOfWork.BlogPostsRepository.GetAllItemsAsync())
            };
        }

        public async Task<GetPostByIdResponseModel> GetPostByIdAsync(int postId)
        {
            return new GetPostByIdResponseModel()
            {
                post = _mapper.Map<BlogPostDto>(await _unitOfWork.BlogPostsRepository.GetPostByIdAsync(postId))
            };
        }
    }
}

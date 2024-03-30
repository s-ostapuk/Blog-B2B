using AutoMapper;
using Blog_Server.Database.Entities;
using Blog_Server.Exceptions;
using Blog_Server.Interfaces.Services;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Models.DtoModels;
using Blog_Server.Models.RequestModels;
using Blog_Server.Models.ResponseModels;
using Blog_Server.Models.ResponseModels.PostsResponseModels;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;

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
        public async Task<BaseResponseModel> GetAllPostsAsync()
        {
            return new BaseResponseModel()
            {
                Data = new GetAllPostsDataResponseModel { blogPosts = _mapper.Map<List<BlogPostDto>>(await _unitOfWork.BlogPostsRepository.GetAllItemsAsync()) }
            };
        }
        
        public async Task<BaseResponseModel> GetPostByIdAsync(int postId)
        {
            return new BaseResponseModel()
            {
                Data = new GetPostByIdDataResponseModel { Post = _mapper.Map<BlogPostDto>(await _unitOfWork.BlogPostsRepository.GetPostByIdAsync(postId)) }
            };
        }
       
        public async Task<BaseResponseModel> CreateNewPostAsync(CreateNewPostRequestModel requestModel, string username)
        {
            var response = new BaseResponseModel();

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null)
            {
                throw new AppException("User not found");
            }

            var post = new BlogPost
            {
                Title = requestModel.Title,
                Content = requestModel.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            try
            {
                await _unitOfWork.BlogPostsRepository.InsertAsync(post);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("Post saving error");
            }
            return response;
        }

        public async Task<BaseResponseModel> DeletePostAsync(int postId, string username)
        {
            var response = new BaseResponseModel();

            var post = await _unitOfWork.BlogPostsRepository.GetAsync(postId);
            if (post is null)
            {
                throw new AppException("Post not found");
            }
            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null || post.UserId != user.Id)
            {
                throw new AppException("Invalid credentials");
            }
            try
            {
                await _unitOfWork.BlogPostsRepository.RemovePostByIdAsync(postId);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("Post deletion error");
            }
            return response;
        }

        public async Task<BaseResponseModel> UpdatePostAsync(UpdatePostRequestModel requestModel, int postId, string username)
        {
            var response = new BaseResponseModel();

            var post = await _unitOfWork.BlogPostsRepository.GetAsync(postId);
            if (post is null)
            {
                throw new AppException("Post not found");
            }
            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null || post.UserId != user.Id)
            {
                throw new AppException("Invalid credentials");
            }

            if (post.UserId != user.Id)
            {
                throw new AppException("Invalid user credentials");
            }

            post.UpdatedAt = DateTime.UtcNow;
            post.Title = requestModel.Title;
            post.Content = requestModel.Content;

            try
            {
                await _unitOfWork.BlogPostsRepository.UpdateAsync(post, true);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("Post update error");
            }

            return response;
        }


    }
}

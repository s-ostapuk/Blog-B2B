using AutoMapper;
using Blog_Server.Database.Entities;
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

            if (requestModel is null || string.IsNullOrEmpty(requestModel.Title) || string.IsNullOrEmpty(requestModel.Content))
            {
                response.Errors.Add("Request model is invalid");
                return response;
            }
            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null)
            {
                response.Errors.Add("User already exists");
                return response;
            }

            var post = new BlogPost
            {
                Title = requestModel.Title,
                Content = requestModel.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            post = await _unitOfWork.BlogPostsRepository.InsertAsync(post);

            if (post is null)
            {
                response.Errors.Add("Insertion error");
                return response;
            }

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                response.Errors.Add("Saving error");
                return response;
            }
            return response;
        }

        public async Task<BaseResponseModel> DeletePostAsync(int postId, string username)
        {
            var response = new BaseResponseModel();

            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }
            var post = await _unitOfWork.BlogPostsRepository.GetAsync(postId);

            if (post is null)
            {
                response.Errors.Add("Post not found");
                return response;
            }
            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null || post.UserId != user.Id)
            {
                response.Errors.Add("Invalid credentials");
                return response;
            }
            try
            {
                await _unitOfWork.BlogPostsRepository.RemovePostByIdAsync(postId);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                response.Errors.Add("Deletion error");
                return response;
            }
            return response;
        }

        public async Task<BaseResponseModel> UpdatePostAsync(UpdatePostRequestModel requestModel, int postId, string username)
        {
            var response = new BaseResponseModel();

            if (requestModel is null || string.IsNullOrEmpty(requestModel.Title) || string.IsNullOrEmpty(requestModel.Content))
            {
                response.Errors.Add("Request model is invalid");
                return response;
            }

            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }

            var post = await _unitOfWork.BlogPostsRepository.GetAsync(postId);
            if (post is null)
            {
                response.Errors.Add("Post not found");
                return response;
            }
            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(username);
            if (user is null || post.UserId != user.Id)
            {
                response.Errors.Add("Invalid credentials");
                return response;
            }

            if (post.UserId != user.Id)
            {
                response.Errors.Add("Invalid user credentials");
                return response;
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
                response.Errors.Add("Update error");
                return response;
            }

            return response;
        }

        public async Task<BaseResponseModel> GetCommentsByPostIdAsync(int postId)
        {
            return new BaseResponseModel()
            {
                Data = new GetCommentsByPostIdModel { Comments = _mapper.Map<List<CommentDto>>(await _unitOfWork.CommentsRepository.GetPostCommentsByPostIdAsync(postId)) }
            };
        }

        public async Task<BaseResponseModel> CreateNewPostCommentAsync(CreateNewPostCommentRequestModel requestModel, int postId, string username)
        {
            var response = new BaseResponseModel();
            if (requestModel is null || string.IsNullOrEmpty(requestModel.CommentText))
            {
                response.Errors.Add("Request model is invalid");
                return response;
            }

            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }

            var comment = new Comment
            {
                CommentText = requestModel.CommentText,
                CreatedAt = DateTime.UtcNow,
                PostId = postId,
                Author = username
            };

            comment = await _unitOfWork.CommentsRepository.InsertAsync(comment);
            if (comment is null)
            {
                response.Errors.Add("Insertion error");
                return response;
            }
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                response.Errors.Add("Saving error");
                return response;
            }
            return response;
        }

        public async Task<BaseResponseModel> UpdatePostCommentAsync(UpdatePostCommentRequestModel requestModel, int postId, int commentId, string username)
        {
            var response = new BaseResponseModel();

            if (requestModel is null || string.IsNullOrEmpty(requestModel.CommentText))
            {
                response.Errors.Add("Request model is invalid");
                return response;
            }

            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }

            var comment = await _unitOfWork.CommentsRepository.GetAsync(commentId);
            if (comment is null)
            {
                response.Errors.Add("Comment not found");
                return response;
            }

            if (comment.PostId != postId || comment.Author != username)
            {
                response.Errors.Add("Invalid comment data or user credentials");
                return response;
            }

            comment.CommentText = requestModel.CommentText;
            comment.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _unitOfWork.CommentsRepository.UpdateAsync(comment, true);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                response.Errors.Add("Update error");
                return response;
            }

            return response;

        }

        public async Task<BaseResponseModel> DeletePostCommentAsync(int postId, int commentId, string username)
        {
            var response = new BaseResponseModel();

            if (string.IsNullOrEmpty(username))
            {
                response.Errors.Add("Invalid user credentials");
                return response;
            }

            var comment = await _unitOfWork.CommentsRepository.GetAsync(commentId);
            if (comment is null)
            {
                response.Errors.Add("Comment not found");
                return response;
            }

            if (comment.PostId != postId || comment.Author != username)
            {
                response.Errors.Add("Invalid comment data or user credentials");
                return response;
            }

            try
            {
                await _unitOfWork.CommentsRepository.RemovePostCommentByPostIdAndIdAsync(postId, commentId);
                await _unitOfWork.CompleteAsync();
            }
            catch 
            {
                response.Errors.Add("Deletion error");
                return response;
            }

            return response;
        }


    }
}

using Blog_Server.Database.Entities;
using Blog_Server.Interfaces.Services;
using Blog_Server.Models.DtoModels;
using Blog_Server.Models.RequestModels;
using Blog_Server.Models.ResponseModels.PostsResponseModels;
using Blog_Server.Models.ResponseModels;
using AutoMapper;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Exceptions;

namespace Blog_Server.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CommentsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

            var comment = new Comment
            {
                CommentText = requestModel.CommentText,
                CreatedAt = DateTime.UtcNow,
                PostId = postId,
                AuthorLogin = username
            };

            try
            {
                await _unitOfWork.CommentsRepository.InsertAsync(comment);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("Comment creation error");
            }
            return response;
        }

        public async Task<BaseResponseModel> UpdatePostCommentAsync(UpdatePostCommentRequestModel requestModel, int postId, int commentId, string username)
        {
            var response = new BaseResponseModel();

            var comment = await _unitOfWork.CommentsRepository.GetAsync(commentId);
            
            if (comment is null)
            {
                throw new AppException("Comment not found");
            }

            if (comment.PostId != postId || comment.AuthorLogin != username)
            {
                throw new AppException("Invalid comment data or user credentials");
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
                throw new AppException("Comment update error");
            }

            return response;

        }

        public async Task<BaseResponseModel> DeletePostCommentAsync(int postId, int commentId, string username)
        {
            var response = new BaseResponseModel();

            var comment = await _unitOfWork.CommentsRepository.GetAsync(commentId);
            
            if (comment is null)
            {
                throw new AppException("Comment not found");
            }

            if (comment.PostId != postId || comment.AuthorLogin != username)
            {
                throw new AppException("Invalid comment data or user credentials");
            }

            try
            {
                await _unitOfWork.CommentsRepository.RemovePostCommentByPostIdAndIdAsync(postId, commentId);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("Comment deletion error");
            }

            return response;
        }
    }
}

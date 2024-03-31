using AutoMapper;
using Blog_Server.Database.Entities;
using Blog_Server.Models.DtoModels;

namespace Blog_Server.Helpers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BlogPost, BlogPostDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<User, UserDto>();
        }
    }
}

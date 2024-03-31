using Blog_Server.Models.DtoModels;
using Blog_Server.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTests.MockData
{
    internal class PostsMockData
    {
        public static BaseResponseModel GetAllPosts()
        {
            return new BaseResponseModel
            {
                Data = new List<BlogPostDto>()
                {
                    new BlogPostDto
                {
                     Id = 1,
                     Title= "TestTitle1",
                     Content= "testcontent1",
                     CreatedAt= DateTime.Parse("2024-03-30T19:19:51.5174584"),
                     UpdatedAt=  DateTime.Parse("2024-03-30T19:19:58.2995635"),
                     UserId= 1,
                     Comments= []
                },
                new BlogPostDto
                {
                     Id = 2,
                     Title= "testtittle2",
                     Content= "testcontent2",
                     CreatedAt= DateTime.Parse("2024-03-30T19:19:51.5174584"),
                     UpdatedAt=  DateTime.Parse("2024-03-30T19:19:58.2995635"),
                     UserId= 1,
                     Comments= []
                },
                new BlogPostDto
                {
                     Id = 3,
                     Title= "testtittle3",
                     Content= "testcontent3",
                     CreatedAt= DateTime.Parse("2024-03-30T19:19:51.5174584"),
                     UpdatedAt=  DateTime.Parse("2024-03-30T19:19:58.2995635"),
                     UserId= 3,
                     Comments= []
                }
                }
            };
        }
        public static BaseResponseModel GetBaseResponseModelWithDataNull()
        {
            return new BaseResponseModel
            {
                Data = null
            };
        }
        public static BaseResponseModel GetBaseResponseModelSuccess()
        {
            return new BaseResponseModel();
        }
        public static BaseResponseModel GetExistingPost()
        {
            return new BaseResponseModel
            {
                Data = new BlogPostDto
                {
                    Id = 2,
                    Title = "testtittle43256",
                    Content = "testcontent13245",
                    CreatedAt = DateTime.Parse("2024-03-30T19:19:51.5174584"),
                    UpdatedAt = DateTime.Parse("2024-03-30T19:19:58.2995635"),
                    UserId = 1,
                    Comments = []
                }
            };
        }

        public static BaseResponseModel GetCommentsByPostIdResponseSuccess()
        {
            return new BaseResponseModel
            {
                Data = new List<CommentDto>()
                {
                    new CommentDto
                {
                     Id = 1,
                     CommentText= "TestTitle1",
                     Author= "testcontent1",
                     CreatedAt= DateTime.Parse("2024-03-30T19:19:51.5174584"),
                     UpdatedAt=  DateTime.Parse("2024-03-30T19:19:58.2995635"),
                     PostId = 2,
                },
                  new CommentDto
                {
                     Id = 2,
                     CommentText= "TestTitle12",
                     Author= "testcontent13",
                     CreatedAt= DateTime.Parse("2024-03-30T19:19:51.5174584"),
                     UpdatedAt=  DateTime.Parse("2024-03-30T19:19:58.2995635"),
                     PostId = 2,
                }
                }
            };
        }
        public static BaseResponseModel GetCommentsByPostIdResponseSuccessEmpty()
        {
            return new BaseResponseModel
            {
                Data = new List<CommentDto>()
            };
        }
    }
}

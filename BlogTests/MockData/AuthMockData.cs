using Blog_Server.Models.AuthModels;
using Blog_Server.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTests.MockData
{
    internal class AuthMockData
    {
        private const string tokenExample = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlRlc3RVc2VyMSIsIm5iZiI6MTcxMTg3OTc2NiwiZXhwIjoxNzEyNDg0NTY2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3Mjk5IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI5OSJ9.1BDfFhLTdYw5wx4bxowNBM-9tmJ64ScBZ0hkMxVOQcw";
        public static BaseResponseModel GetExistingCredentialsTokenData()
        {
            return new BaseResponseModel
            {
                Data = new AuthResponseDataModel { AccessToken = tokenExample }
            };

        }
        public static BaseResponseModel GetInvalidCredentialsTokenData()
        {
            return new BaseResponseModel
            {
                Data = null,
                Errors = new List<string>() { "Invalid login or password" },
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

        }
        public static BaseResponseModel GetRegisterNewUserSuccessResponse()
        {
            return new BaseResponseModel();
        }
    }
}

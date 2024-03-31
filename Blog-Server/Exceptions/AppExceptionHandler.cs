using Blog_Server.Models.ResponseModels;
using Microsoft.AspNetCore.Diagnostics;

namespace Blog_Server.Exceptions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 400;
            httpContext.Response.ContentType = "application/json";
            var response = new BaseResponseModel();
            response.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
            response.Errors.Add(exception.Message);
            await httpContext.Response.WriteAsJsonAsync(response);
            return true;
        }
    }
}

using System.Text;

namespace Blog_Server.Middleware
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpLoggingMiddleware> _logger;

        public HttpLoggingMiddleware(RequestDelegate next,
        ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);
            await _next(context);
            var req = context.Request;
            req.EnableBuffering();
            LogResponse(context);
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;
            var requestLog = new StringBuilder();
            requestLog.AppendLine($"Incoming Request(UTC time: {DateTime.UtcNow.ToLongTimeString()}):");
            requestLog.AppendLine($"HTTP {request.Method} {request.Path}");
            requestLog.AppendLine($"Host: {request.Host}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");

            _logger.LogInformation(requestLog.ToString());
        }

        private void LogResponse(HttpContext context)
        {
            var response = context.Response;

            var responseLog = new StringBuilder();
            responseLog.AppendLine($"Outgoing Response(UTC time: {DateTime.UtcNow.ToLongTimeString()}):");
            responseLog.AppendLine($"HTTP {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            _logger.LogInformation(responseLog.ToString());
        }

    }
}

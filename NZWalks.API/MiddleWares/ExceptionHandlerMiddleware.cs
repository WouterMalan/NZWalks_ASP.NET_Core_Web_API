using System.Net;

namespace NZWalks.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                //log the error
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                //return a customer error response
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    ErrorId = errorId,
                    Message = "An error occurred while processing your request. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }

    }
}
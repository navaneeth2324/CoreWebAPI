using System.Net;

namespace EmployeeAdminPortal.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,RequestDelegate next)
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
                var errorid = Guid.NewGuid().ToString();
                logger.LogError(ex,$"{errorid} : {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var error= new
                {
                    ErrorId = errorid,
                    ErrorMessage = "Something went wrong, Please try again later"
                };
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}

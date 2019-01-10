using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

//Original source code by
//https://stackoverflow.com/a/38935583/4601817

namespace BankingApp.WebAPI
{
    /// <summary>
    /// Error Handling Middleware 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invoking next delegate
        /// </summary>
        /// <param name="context">HTTP Context</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">HTTP Context</param>
        /// <param name="exception">Exception details</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 is by default

            //Custom codes
            //if (exception is NotFoundException) code = HttpStatusCode.NotFound;

            //Render error as JSON
            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}

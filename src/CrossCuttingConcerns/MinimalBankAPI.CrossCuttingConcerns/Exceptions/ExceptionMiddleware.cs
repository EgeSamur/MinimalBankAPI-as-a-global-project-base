using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace MinimalBankAPI.CrossCuttingConcerns.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public ExceptionMiddleware()
        {
            // JsonSerializerOptions yapılandırması
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true, // Logları daha okunabilir hale getirir,
            };
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);  // Request'i bir sonraki middleware'e gönder
            }
            catch (Exception ex)
            {
                // exception handler 
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task<string> GetRequestBody(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            httpContext.Request.Body.Position = 0;
            using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
            string content = await reader.ReadToEndAsync();
            httpContext.Request.Body.Position = 0;
            return content;
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;


            List<string> errors = new()
            {
                exception.Message,
            };
            var y = new ExceptionModel
            {
                Errors = errors,
                StatusCode = statusCode

            }.ToString();
            var x = httpContext.Response.WriteAsync(y);
            return x;
        }

        private static int GetStatusCode(Exception exception) =>

            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

    }

}

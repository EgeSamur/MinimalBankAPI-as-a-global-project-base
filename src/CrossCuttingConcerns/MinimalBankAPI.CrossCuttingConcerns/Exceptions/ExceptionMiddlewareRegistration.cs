using Microsoft.AspNetCore.Builder;

namespace MinimalBankAPI.CrossCuttingConcerns.Exceptions
{
    public static class ExceptionMiddlewareRegistration
    {

        public static void AddConfigureGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }

}

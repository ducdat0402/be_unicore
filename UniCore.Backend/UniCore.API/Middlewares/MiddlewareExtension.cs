using UniCore.API.Extensions;
using UniCore.API.Middlewares.CustomMiddlewares;

namespace UniCore.API.Middlewares
{
    public static class MiddlewareExtension
    {
        public static void ConfigureMiddlewarePipeline(this WebApplication app, IWebHostEnvironment env)
        {
            // Diagnostics & Error Handling
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseExceptionHandler();
            app.UseRequestLocalization();
            app.UseDeveloperExceptionPage();

            // Security & Redirection
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.ConfigureCors(env);
            app.ConfigureForwardedHeadersOptions();
            app.ConfigureRateLimiting();

            // Routing & Authorization
            app.ConfigureAuthentication(env);
            app.ConfigureAuthorization(env);

            // Endpoints
            app.MapHealthChecks("/health");
            app.MapControllers();
        }
    }
}

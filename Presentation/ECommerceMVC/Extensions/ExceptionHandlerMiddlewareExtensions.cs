using ECommerceMVC.Filters;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}

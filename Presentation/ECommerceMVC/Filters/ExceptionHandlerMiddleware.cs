using ECommerce.Application.Abstractions.Services;
using static Google.Apis.Requests.BatchRequest;

namespace ECommerceMVC.Filters
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ExceptionHandlerMiddleware> logger;
		public ExceptionHandlerMiddleware(RequestDelegate Next,
			                              ILogger<ExceptionHandlerMiddleware> Logger)
		{
			next = Next;
			logger = Logger;
		}
		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
			}
			catch (Exception e)
			{


                httpContext.Response.Cookies.Append("toast", $"danger|{e.Message}", new Microsoft.AspNetCore.Http.CookieOptions
				{
					HttpOnly = false,
					Expires = DateTime.Now.AddDays(1)
				});

				string actionName = httpContext.Request.Path;
				logger.LogError($"{actionName} --- {e.Message}"); // dont show exception message to cilent

				httpContext.Response.Redirect("/");

            }
		}
	}
}

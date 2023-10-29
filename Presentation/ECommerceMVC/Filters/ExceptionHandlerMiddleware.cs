using ECommerce.Application.Abstractions.Services;

namespace ECommerceMVC.Filters
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IMailService _mailService;
		public ExceptionHandlerMiddleware(RequestDelegate Next,
			                              IMailService mailService)
		{
			next = Next;
			_mailService = mailService;
		}
		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
			}
			catch (Exception e)
			{
				await _mailService.SendMailAsync("ganbarovelcin@gmail.com", "Exception error", e.Message);
			}
		}
	}
}

using ECommerce.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web;
using ECommerce.Application.Helpers;

namespace ECommerce.Infrastucture.Services
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public MailService(IConfiguration configuration,
						   IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
		{
			await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
		}

		public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
		{
			MailMessage mail = new();
			mail.IsBodyHtml = isBodyHtml;
			foreach (var to in tos)
				mail.To.Add(to);
			mail.Subject = subject;
			mail.Body = body;
			mail.From = new(_configuration["Mail:Username"], "DG Product", System.Text.Encoding.UTF8);

			SmtpClient smtp = new("smtp.gmail.com", 587);
			smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
			smtp.Port = 587;
			smtp.EnableSsl = true;
			await smtp.SendMailAsync(mail);
		}

		public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
		{
			var request = _httpContextAccessor.HttpContext.Request;
			string baseUrl = $"{request.Scheme}://{request.Host.Value}";

			string resetUrl = $"{baseUrl}/auth/updatepassword/{userId}";

			StringBuilder mail = new StringBuilder();
			mail.AppendLine("Hello,<br>If you have requested a new password, you can renew your password from the link below.<br><strong><a target=\"_blank\" href=\"");
			mail.AppendLine(resetUrl);
			mail.AppendLine("\">Click to request a new password...</a></strong><br><br><span style=\"font-size:12px;\">NOTE: If this request has not been fulfilled by you, please do not take this e-mail seriously.</span><br>Respectfully...<br><br><br>NG - Sales Product");

			await SendMailAsync(to, "Password Reset Request", mail.ToString());
		}


		public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
		{
			string mail = $"Dear {userName} Hello<br>" +
				$"Your order with code {orderCode}, which you placed on {orderDate}, has been completed and given to the cargo company.";

			await SendMailAsync(to, $"{orderCode} Your Order with Order Number is Completed", mail);

		}

	}

}

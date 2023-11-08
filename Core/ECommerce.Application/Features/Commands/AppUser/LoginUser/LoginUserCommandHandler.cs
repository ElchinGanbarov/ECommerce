using ECommerce.Application.Abstractions.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginUserCommandHandler(IAuthService authService,
            IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var experationDateTime = request.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1);
            var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, experationDateTime);
            if(token  is  null) 
            {
                return  new LoginUserCommandResponse { Success = false  };
            }
            return new LoginUserCommandResponse()
            {
                Token = token,
                Message= $"{request.UsernameOrEmail}",
                Success = true
            };
        }
    }
}
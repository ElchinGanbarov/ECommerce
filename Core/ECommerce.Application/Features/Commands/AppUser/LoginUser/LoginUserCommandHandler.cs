using ECommerce.Application.Abstractions.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;
        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var experationDateTime = request.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1);
            var response = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, experationDateTime);
            return new LoginUserCommandResponse(response.Data, response.Success, response.Message);

        }
    }
}
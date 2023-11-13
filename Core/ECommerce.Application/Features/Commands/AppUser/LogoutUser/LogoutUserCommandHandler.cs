using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.AppUser.LogoutUser
{

    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommandRequest, LogoutUserCommandResponse>
    {
        readonly IAuthService _authService;
        public LogoutUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LogoutUserCommandResponse> Handle(LogoutUserCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LogoutAsync();
            return new LogoutUserCommandResponse (result.Success,result.Message);
        }
    }
}

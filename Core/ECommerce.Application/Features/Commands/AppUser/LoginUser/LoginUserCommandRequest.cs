using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

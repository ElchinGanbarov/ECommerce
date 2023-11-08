using ECommerce.Application.DTOs;
using ECommerce.Application.Results;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse : IResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }
        public Token Token { get; set; }

    }

}

using ECommerce.Application.DTOs;
using ECommerce.Application.Results;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse : DataResult<Token>
    {
        public LoginUserCommandResponse(Token data, bool success, string message) : base(data, success, message)
        {
        }
    }

}

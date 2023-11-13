using ECommerce.Application.Results;

namespace ECommerce.Application.Features.Commands.AppUser.LogoutUser
{
    public class LogoutUserCommandResponse : Result
    {
        public LogoutUserCommandResponse(bool success, string message) : base(success, message)
        {
        }
    }   
}
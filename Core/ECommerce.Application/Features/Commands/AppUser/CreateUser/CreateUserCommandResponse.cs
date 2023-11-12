using ECommerce.Application.Results;

namespace ECommerce.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandResponse : Result
    {
        public CreateUserCommandResponse(bool success, string message) : base(success, message)
        {
        }
    }
}

using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.LogoutUser
{
    public class LogoutUserCommandRequest : IRequest<LogoutUserCommandResponse>
    {
    }
}
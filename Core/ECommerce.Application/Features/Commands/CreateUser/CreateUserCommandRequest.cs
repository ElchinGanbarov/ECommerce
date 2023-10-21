using MediatR;

namespace ECommerce.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public required string NameSurname { get; set; }
        public required string Username { get; set; }
        public required  string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirm { get; set; }
    }
}
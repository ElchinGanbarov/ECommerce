using MediatR;

namespace ECommerce.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public required string NameSurname { get; set; }
        public  string Username 
        {
            get { return Username; }
            set { Username = NameSurname; }
        }
        public required  string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirm { get; set; }
    }
}
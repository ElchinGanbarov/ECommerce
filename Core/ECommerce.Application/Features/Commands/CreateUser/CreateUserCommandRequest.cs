using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public required string NameSurname { get; set; }
        public required  string Email { get; set; }
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long.")]
        public required string Password { get; set; }
    }
}
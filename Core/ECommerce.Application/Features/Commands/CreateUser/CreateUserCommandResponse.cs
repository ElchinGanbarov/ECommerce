using ECommerce.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandResponse : Result
    {
        public CreateUserCommandResponse(bool success, string message) : base(success, message)
        {
        }
    }
}

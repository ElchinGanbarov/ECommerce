using ECommerce.Application.Results;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Products.CreateProduct
{

    public record CreateProductCommandRequest(string Name,int Stock,float Price) : IRequest<CreateProductCommandResponse>;


    public class CreateProductCommandResponse : Result
    {
        public CreateProductCommandResponse( bool success, string message) : base( success, message)
        {
        }
    }
}

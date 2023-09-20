using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Products.CreateProduct
{
    public  class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public required string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }



    public class CreateProductCommandResponse
    {

    }
}

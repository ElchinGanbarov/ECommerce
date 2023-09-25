using AutoMapper;
using ECommerce.Application.Repositories.Products;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public CreateProductCommandHandler(IMapper mapper,
                                           IProductWriteRepository productWriteRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productWriteRepository = productWriteRepository ?? throw new ArgumentNullException(nameof(productWriteRepository));
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productWriteRepository.AddAsync(new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = Guid.Parse("c41121ed-b6fb-c9a6-bc9b-574c82929e7e")
            }); ;
            await _productWriteRepository.SaveAsync();
            return new();
        }


        
    }
}

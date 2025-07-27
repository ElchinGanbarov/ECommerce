using ECommerce.Application.Extensions;
using ECommerce.Application.Repositories.Products;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.GetAllProduct
{
    internal class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;

        private readonly IMemoryCache _cache;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, IMemoryCache cache)
        {
            _productReadRepository = productReadRepository;
            _cache = cache;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var model = await _cache.GetOrCreateAsync(request.cacheKey, entry =>
            {
                return GetAllProduct();
            }, false, 50, 50);


            return  new GetAllProductQueryResponse(model, true, "okay");
        }


        private async Task<List<Product>> GetAllProduct()
        {
            var products =  _productReadRepository.GetAll().ToList();
            return products;
        }


    }
}

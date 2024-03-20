using ECommerce.Application.DTOs;
using ECommerce.Application.Results;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.GetAllProduct
{
    public class GetAllProductQueryResponse : DataResult<List<Product>>
    {
        public GetAllProductQueryResponse(List<Product> data, bool success, string message) : base(data, success, message)
        {
        }
    }
}

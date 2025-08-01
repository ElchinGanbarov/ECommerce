﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.GetAllProduct
{
    public record GetAllProductQueryRequest(string cacheKey, string Name = "") : IRequest<GetAllProductQueryResponse>;
    
}

﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.GetAllProduct
{
    internal class SearchProductQueryHandler : IRequestHandler<SearchProductQueryRequest, SearchProductQueryResponse>
    {
        public Task<SearchProductQueryResponse> Handle(SearchProductQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Results
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}

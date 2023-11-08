using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Results
{
    public interface IDataResult<out T>:IResult
    {
        T Data { get; }
    }
}

using Swastika.IO.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.UI.Base
{
    public class ApiHelper<T>
    {
        public static ApiResult<T> GetResult(int status, T data, string responseKey
            , List<string> errors)
        {
            ApiResult<T> result = new ApiResult<T>()
            {
                Status = status,
                ResponseKey = responseKey,
                Data = data,
                Errors = errors,
            };

            return result;
        }
    }
}

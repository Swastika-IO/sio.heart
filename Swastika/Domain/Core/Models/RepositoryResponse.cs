using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Domain.Core.Models
{
    public class RepositoryResponse<TResult>
    {
        public bool IsSucceed { get; set; }
        public TResult Data { get; set; }
        public Exception Ex { get; set; }
    }
}

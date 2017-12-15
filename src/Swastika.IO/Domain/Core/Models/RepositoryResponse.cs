using Newtonsoft.Json.Linq;
using Swastika.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Swastika.Domain.Core.Models
{
    public class RepositoryResponse<TResult>
    {
        public bool IsSucceed { get; set; }
        public TResult Data { get; set; }
        public Exception Ex { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class PaginationModel<T>
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        public List<JObject> JsonItems { get; set; } = new List<JObject>();
        public PaginationModel()
        {
            PageIndex = 0;
            PageSize = 0;
            TotalItems = 0;
            TotalPage = 1;
            Items = new List<T>();
        }
    }    
}

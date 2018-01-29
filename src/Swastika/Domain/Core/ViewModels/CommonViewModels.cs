using Microsoft.Data.OData.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Domain.Core.ViewModels
{
    public class ApiResult<T>
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();
        [JsonProperty("exception")]
        public Exception Exception { get; set; }

    }

    public class FileStreamViewModel
    {
        public string Base64 { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
    public class RequestPaging
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Keyword { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public int CountryId { get; set; }
        public int PageIndex { get; set; } = 0;
        public int? PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Id";
        public Microsoft.Data.OData.Query.OrderByDirection Direction { get; set; } = OrderByDirection.Ascending;
    }
    public class RepositoryResponse<TResult>
    {
        [JsonProperty("isSucceed")]
        public bool IsSucceed { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }
        [JsonProperty("data")]
        public TResult Data { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();
        [JsonProperty("exception")]
        public Exception Exception { get; set; }
    }
    public class EntityField
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
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

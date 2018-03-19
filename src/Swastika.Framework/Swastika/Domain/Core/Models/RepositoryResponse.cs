using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Swastika.Domain.Core.Models
{
    public class RepositoryResponse<TResult>
    {
        [JsonProperty("isSucceed")]
        public bool IsSucceed { get; set; }
        [JsonProperty("data")]
        public TResult Data { get; set; }
        [JsonProperty("ex")]
        public Exception Ex { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class PaginationModel<T>
    {

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("items")]
        public List<T> Items { get; set; }
        [JsonProperty("jsonItems")]
        public List<JObject> JsonItems { get; set; } = new List<JObject>();
        public PaginationModel()
        {
            PageIndex = 0;
            PageSize = 1;
            TotalItems = 0;
            TotalPage = 1;
            Items = new List<T>();
        }
    }    
}

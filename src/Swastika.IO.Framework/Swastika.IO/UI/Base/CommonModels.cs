using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.UI.Base
{
    public class SWConstants
    {
        public enum ResponseKey
        {
            BadRequest = 400,
            NotFound = 404,
            OK = 200
        }
    }

    public class ApiResult<T>
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("errors")]
        public IEnumerable<string> Errors { get; set; }
    }
    
    public class RequestPaging
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Keyword { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}

using Microsoft.Data.OData.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Swastika.Domain.Core.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        [JsonProperty("exception")]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the response key.
        /// </summary>
        /// <value>
        /// The response key.
        /// </value>
        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class EntityField
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The property value.
        /// </value>
        public string PropertyValue { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class FileStreamViewModel
    {
        /// <summary>
        /// Gets or sets the base64.
        /// </summary>
        /// <value>
        /// The base64.
        /// </value>
        public string Base64 { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationModel{T}"/> class.
        /// </summary>
        public PaginationModel()
        {
            PageIndex = 0;
            PageSize = 0;
            TotalItems = 0;
            TotalPage = 1;
            Items = new List<T>();
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the json items.
        /// </summary>
        /// <value>
        /// The json items.
        /// </value>
        public List<JObject> JsonItems { get; set; } = new List<JObject>();

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        /// <value>
        /// The total items.
        /// </value>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total page.
        /// </summary>
        /// <value>
        /// The total page.
        /// </value>
        public int TotalPage { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class RepositoryResponse<TResult>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("data")]
        public TResult Data { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        [JsonProperty("exception")]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is succeed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is succeed; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("isSucceed")]
        public bool IsSucceed { get; set; }

        /// <summary>
        /// Gets or sets the response key.
        /// </summary>
        /// <value>
        /// The response key.
        /// </value>
        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class RequestPaging
    {
        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        /// <value>
        /// The country identifier.
        /// </value>
        public int CountryId { get; set; }
        [JsonProperty("pageIndex")]
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public OrderByDirection Direction { get; set; } = OrderByDirection.Ascending;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public string OrderBy { get; set; } = "Id";

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int? PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>
        /// The user agent.
        /// </value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }
    }
}
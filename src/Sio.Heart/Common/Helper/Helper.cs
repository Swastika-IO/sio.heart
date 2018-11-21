// Licensed to the Siocore Foundation under one or more agreements.
// The Siocore Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Sio.Domain.Core.ViewModels;
using System.Collections.Generic;

namespace Sio.UI.Base
{
    /// <summary>
    /// Api Helper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ApiHelper<T>
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="data">The data.</param>
        /// <param name="responseKey">The response key.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static RepositoryResponse<T> GetResult(int status, T data, string responseKey, List<string> errors)
        {
            RepositoryResponse<T> result = new RepositoryResponse<T>()
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

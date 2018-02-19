// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swastika.Domain.Core.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class ExpandViewModelBase<TDbContext, TModel>
    where TDbContext : DbContext
    where TModel : class
    {
        /// <summary>
        /// The errors
        /// </summary>
        [JsonIgnore]
        public List<string> errors = new List<string>();

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        [JsonIgnore]
        public bool IsValid = true;

        /// <summary>
        /// Gets or sets the list supported culture.
        /// </summary>
        /// <value>
        /// The list supported culture.
        /// </value>
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [JsonIgnore]
        public TModel Model { get; set; }

        /// <summary>
        /// Gets or sets the response key.
        /// </summary>
        /// <value>
        /// The response key.
        /// </value>
        [JsonIgnore]
        public string ResponseKey { get; set; }

        /// <summary>
        /// Expands the model.
        /// </summary>
        /// <param name="model">The model.</param>
        public virtual void ExpandModel(TModel model)
        {
        }

        /// <summary>
        /// Expands the view.
        /// </summary>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        public virtual void ExpandView(TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
        }

        /// <summary>
        /// Removes the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public virtual Task<bool> RemoveModelAsync(TModel model, TDbContext _context, IDbContextTransaction _transaction)
        {
            return default(Task<bool>);
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="isSaveSubModels">if set to <c>true</c> [is save sub models].</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public abstract bool SaveModel(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Saves the model asynchronous.
        /// </summary>
        /// <param name="isSaveSubModels">if set to <c>true</c> [is save sub models].</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public abstract Task<bool> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Saves the sub models.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public virtual bool SaveSubModels(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return false;
        }

        /// <summary>
        /// Saves the sub models asynchronous.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public virtual Task<bool> SaveSubModelsAsync(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public virtual void Validate()
        {
        }
    }
}
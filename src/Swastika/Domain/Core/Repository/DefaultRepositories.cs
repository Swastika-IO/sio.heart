// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;

namespace Swastika.Domain.Data.Repository
{
    /// <summary>
    /// Default Repository
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="Swastika.Domain.Data.Repository.ModelRepositoryBase{TContext, TModel}" />
    public class DefaultRepository<TContext, TModel> : ModelRepositoryBase<TContext, TModel>
        where TContext : DbContext
        where TModel : class
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile DefaultRepository<TContext, TModel> instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultRepository{TContext, TModel}"/> class from being created.
        /// </summary>
        private DefaultRepository()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DefaultRepository<TContext, TModel> Instance {
            get {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultRepository<TContext, TModel>();
                    }
                }

                return instance;
            }
        }
    }

    /// <summary>
    /// Default Repository with view
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <seealso cref="Swastika.Domain.Data.Repository.ModelRepositoryBase{TContext, TModel}" />
    public class DefaultRepository<TDbContext, TModel, TView> :
        Swastika.Domain.Data.Repository.ViewRepositoryBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : Swastika.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile DefaultRepository<TDbContext, TModel, TView> instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultRepository{TDbContext, TModel, TView}"/> class from being created.
        /// </summary>
        private DefaultRepository()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DefaultRepository<TDbContext, TModel, TView> Instance {
            get {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultRepository<TDbContext, TModel, TView>();
                    }
                }

                return instance;
            }
        }
    }
}

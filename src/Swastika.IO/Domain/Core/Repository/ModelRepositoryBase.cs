using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swastika.Common.Helper;
using Swastika.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Data.OData.Query;
using Swastika.IO.Domain.Core.ViewModels;

namespace Swastika.Domain.Data.Repository
{

    /// <summary>
    /// Base Repository
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TModel">The type of the view.</typeparam>
    /// <typeparam name="TDbContext">The type of the context.</typeparam>
    /// <seealso cref="Swastika.Extension.Blog.Interfaces.IRepository{TModel, TModel}" />
    public abstract class ModelRepositoryBase<TDbContext, TModel>
        where TModel : class where TDbContext : DbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TModel, TModel, TContext}"/> class.
        /// </summary>
        public ModelRepositoryBase()
        {
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <returns></returns>
        public virtual TDbContext InitContext()
        {
            Type classType = typeof(TDbContext);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
            TDbContext context = (TDbContext)classConstructor.Invoke(new object[] { });

            return context;
        }

        /// <summary>
        /// Determines whether the specified entity is exists.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CheckIsExists(TModel entity, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                //For the former case use:
                return context.Set<TModel>().Any(e => e == entity);

                //For the latter case use(it will check loaded entities as well):
                //return (_context.Set<T>().Find(keys) != null);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    transaction.Rollback();
                }
                return false;
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Determines whether the specified predicate is exists.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if the specified predicate is exists; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckIsExists(System.Func<TModel, bool> predicate, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                //For the former case use:
                return context.Set<TModel>().Any(predicate);

                //For the latter case use(it will check loaded entities as well):
                //return (_context.Set<T>().Find(keys) != null);
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    transaction.Rollback();
                }
                return false;
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<TModel> CreateModel(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                context.Entry(model).State = EntityState.Added;
                bool result = context.SaveChanges() > 0;
                if (result && isSaveSubModels)
                {
                    result = SaveSubModel(model, context, transaction);
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        transaction.Commit();
                    }

                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        transaction.Rollback();
                    }

                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = null
                    };
                }

            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    transaction.Rollback();
                }
                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<TModel>> CreateModelAsync(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                context.Entry(model).State = EntityState.Added;
                bool result = await context.SaveChangesAsync() > 0;
                if (result && isSaveSubModels)
                {
                    result = await SaveSubModelAsync(model, context, transaction);
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }

                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = null
                    };
                }


            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = null
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Edits the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<TModel> EditModel(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                context.Entry(model).State = EntityState.Modified;
                bool result = context.SaveChanges() > 0;

                if (result && isSaveSubModels)
                {
                    result = SaveSubModel(model, context, transaction);
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = null
                    };
                }


            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Edits the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<TModel>> EditModelAsync(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                context.Entry(model).State = EntityState.Modified;
                bool result = await context.SaveChangesAsync() > 0;
                if (result && isSaveSubModels)
                {
                    result = await SaveSubModelAsync(model, context, transaction);
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = null
                    };
                }
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        #region GetModelList


        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual RepositoryResponse<List<TModel>> GetModelList(TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var lstModel = context.Set<TModel>().ToList();

                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = true,
                    Data = lstModel
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }

        }

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual RepositoryResponse<PaginationModel<TModel>> GetModelList(
            string orderByPropertyName, OrderByDirection direction, int? pageSize, int? pageIndex,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                dynamic orderBy = GetLambda(orderByPropertyName);
                List<TModel> lstModel = new List<TModel>();
                var query = context.Set<TModel>();
                IQueryable<TModel> sorted = null;
                PaginationModel<TModel> result = new PaginationModel<TModel>()
                {
                    TotalItems = query.Count(),
                    PageIndex = pageIndex ?? 0
                };
                result.PageSize = pageSize ?? result.TotalItems;

                if (pageSize.HasValue)
                {
                    result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                }

                switch (direction)
                {

                    case OrderByDirection.Descending:
                        sorted = Queryable.OrderByDescending(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = sorted
                                .Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToList();
                        }
                        else
                        {
                            lstModel = sorted.ToList();
                        }
                        break;

                    default:
                        sorted = Queryable.OrderBy(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value).ToList();
                        }
                        else
                        {
                            lstModel = sorted.ToList();
                        }
                        break;
                }

                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                result.Items = lstModel;

                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<List<TModel>>> GetModelListAsync(
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var lstModel = await context.Set<TModel>().ToListAsync();
                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = true,
                    Data = lstModel
                };

            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }

        }

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>

        public virtual async Task<RepositoryResponse<PaginationModel<TModel>>> GetModelListAsync(
            string orderByPropertyName, OrderByDirection direction, int? pageSize, int? pageIndex,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                dynamic orderBy = GetLambda(orderByPropertyName);
                List<TModel> lstModel = new List<TModel>();
                var query = context.Set<TModel>();
                IQueryable<TModel> sorted = null;
                PaginationModel<TModel> result = new PaginationModel<TModel>()
                {
                    TotalItems = query.Count(),
                    PageIndex = pageIndex ?? 0
                };
                result.PageSize = pageSize ?? result.TotalItems;

                if (pageSize.HasValue)
                {
                    result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                }

                switch (direction)
                {
                    case OrderByDirection.Descending:
                        sorted = Queryable.OrderByDescending(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            

                            lstModel = await sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToListAsync();
                        }
                        else
                        {
                            lstModel = await Queryable.OrderByDescending(query, orderBy)
                                .ToListAsync();
                        }
                        break;

                    default:
                        sorted = Queryable.OrderBy(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = await sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToListAsync();
                        }
                        else
                        {
                            lstModel = await sorted.ToListAsync();
                        }
                        break;
                }
                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                result.Items = lstModel;

                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }


        #endregion GetModelList

        #region GetModelListBy

        /// <summary>
        /// Gets the model list by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual RepositoryResponse<List<TModel>> GetModelListBy(Expression<Func<TModel, bool>> predicate,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var lstModel = context.Set<TModel>().Where(predicate).ToList();
                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = true,
                    Data = lstModel
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }

        }

        /// <summary>
        /// Gets the model list by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual RepositoryResponse<PaginationModel<TModel>> GetModelListBy(
            Expression<Func<TModel, bool>> predicate, string orderByPropertyName, OrderByDirection direction, int? pageSize, int? pageIndex,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                dynamic orderBy = GetLambda(orderByPropertyName);
                List<TModel> lstModel = new List<TModel>();
                var query = context.Set<TModel>().Where(predicate);
                IQueryable<TModel> sorted = null;
                PaginationModel<TModel> result = new PaginationModel<TModel>()
                {
                    TotalItems = query.Count(),
                    PageIndex = pageIndex ?? 0
                };
                result.PageSize = pageSize ?? result.TotalItems;

                if (pageSize.HasValue)
                {
                    result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                }

                switch (direction)
                {
                    case OrderByDirection.Descending:
                        sorted = Queryable.OrderByDescending(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            

                            lstModel = sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToList();
                        }
                        else
                        {
                            lstModel = sorted.ToList();
                        }
                        break;

                    default:
                        sorted = Queryable.OrderBy(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToList();
                        }
                        else
                        {
                            lstModel = sorted.ToList();
                        }
                        break;
                }

                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                result.Items = lstModel;

                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the model list by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<List<TModel>>> GetModelListByAsync(
            Expression<Func<TModel, bool>> predicate,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var lstModel = await context.Set<TModel>().Where(predicate).ToListAsync();
                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = true,
                    Data = lstModel
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the model list by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<PaginationModel<TModel>>> GetModelListByAsync(
            Expression<Func<TModel, bool>> predicate, string orderByPropertyName, OrderByDirection direction,
            int? pageSize, int? pageIndex,
            TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                dynamic orderBy = GetLambda(orderByPropertyName);
                List<TModel> lstModel = new List<TModel>();
                var query = context.Set<TModel>().Where(predicate);
                IQueryable<TModel> sorted = null;

                PaginationModel<TModel> result = new PaginationModel<TModel>()
                {
                    TotalItems = query.Count(),
                    PageIndex = pageIndex ?? 0
                };
                result.PageSize = pageSize ?? result.TotalItems;

                if (pageSize.HasValue)
                {
                    result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                }
                switch (direction)
                {
                    case OrderByDirection.Descending:
                        sorted = Queryable.OrderByDescending(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = await sorted.Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToListAsync();
                        }
                        else
                        {
                            lstModel = await sorted.ToListAsync();
                        }
                        break;

                    default:
                        sorted = Queryable.OrderBy(query, orderBy);
                        if (pageSize.HasValue)
                        {
                            lstModel = await sorted
                                .Skip(pageIndex.Value * pageSize.Value)
                                .Take(pageSize.Value)
                                .ToListAsync();
                        }
                        else
                        {
                            lstModel = await sorted.ToListAsync();
                        }
                        break;
                }

                lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                result.Items = lstModel;

                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = true,
                    Data = result
                };
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<PaginationModel<TModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }


        #endregion GetModelListBy

        /// <summary>
        /// Gets the single model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual RepositoryResponse<TModel> GetSingleModel(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                TModel model = context.Set<TModel>().FirstOrDefault(predicate);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Detached;
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = model
                    };
                }
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = default(TModel)
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the single model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<TModel>> GetSingleModelAsync(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                TModel model = await context.Set<TModel>().FirstOrDefaultAsync(predicate);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Detached;

                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = true,
                        Data = model
                    };
                }
                else
                {
                    return new RepositoryResponse<TModel>()
                    {
                        IsSucceed = false,
                        Data = model
                    };
                }
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<TModel>()
                {
                    IsSucceed = false,
                    Data = default(TModel)
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the list model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<bool> RemoveListModel(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var models = context.Set<TModel>().Where(predicate).ToList();
                bool result = true;
                if (models != null)
                {
                    foreach (var model in models)
                    {
                        if (result)
                        {
                            var r = RemoveModel(model, context, transaction);
                            result = result && r.IsSucceed;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (result)
                    {
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = true,
                            Data = true
                        };
                    }
                    else
                    {
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Rollback();
                        }
                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = false,
                            Data = false
                        };
                    }
                }
                else
                {
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the list model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<bool>> RemoveListModelAsync(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                var models = await context.Set<TModel>().Where(predicate).ToListAsync();
                bool result = true;
                if (models != null)
                {
                    foreach (var model in models)
                    {
                        if (result)
                        {
                            var r = await RemoveModelAsync(model, context, transaction);
                            result = result && r.IsSucceed;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (result)
                    {
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = true,
                            Data = true
                        };
                    }
                    else
                    {
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Rollback();
                        }
                        return new RepositoryResponse<bool>()
                        {
                            IsSucceed = false,
                            Data = false
                        };
                    }
                }
                else
                {
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<bool> RemoveModel(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)

        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                TModel model = context.Set<TModel>().FirstOrDefault(predicate);
                bool result = true;
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    result = context.SaveChanges() > 0;
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false
                    };
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<bool> RemoveModel(TModel model
            , TDbContext _context = null, IDbContextTransaction _transaction = null)

        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                bool result = true;
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    result = context.SaveChanges() > 0;
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false
                    };
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<bool>> RemoveModelAsync(Expression<Func<TModel, bool>> predicate
            , TDbContext _context = null, IDbContextTransaction _transaction = null)

        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                TModel model = await context.Set<TModel>().FirstOrDefaultAsync(predicate);
                bool result = true;
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    result = await context.SaveChangesAsync() > 0;
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false
                    };
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        // TODO: Should return return enum status code instead
        /// <summary>
        /// Removes the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual async Task<RepositoryResponse<bool>> RemoveModelAsync(TModel model
            , TDbContext _context = null, IDbContextTransaction _transaction = null)

        {
            TDbContext context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                bool result = true;
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    result = await context.SaveChangesAsync() > 0;
                }

                if (result)
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Commit();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = true,
                        Data = true
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false
                    };
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }

                return new RepositoryResponse<bool>()
                {
                    IsSucceed = false,
                    Data = false,
                    Exception = ex
                };
            }
            finally
            {
                if (_context == null)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual RepositoryResponse<TModel> SaveModel(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CheckIsExists(model, _context, _transaction))
            {
                return EditModel(model, isSaveSubModels, _context, _transaction);
            }
            else
            {
                return CreateModel(model, isSaveSubModels, _context, _transaction);
            }
        }

        /// <summary>
        /// Saves the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual Task<RepositoryResponse<TModel>> SaveModelAsync(TModel model, bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CheckIsExists(model, _context, _transaction))
            {
                return EditModelAsync(model, isSaveSubModels, _context, _transaction);
            }
            else
            {
                return CreateModelAsync(model, isSaveSubModels, _context, _transaction);
            }
        }

        public virtual bool SaveSubModel(TModel model, TDbContext context, IDbContextTransaction _transaction)
        {
            return false;
        }

        /// <summary>
        /// Saves the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual Task<bool> SaveSubModelAsync(TModel model, TDbContext context, IDbContextTransaction _transaction)
        {
            return default(Task<bool>);
        }

        protected LambdaExpression GetLambda(string propName)
        {
            var parameter = Expression.Parameter(typeof(TModel));
            var memberExpression = Expression.Property(parameter, propName);
            return Expression.Lambda(memberExpression, parameter);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void LogErrorMessage(Exception ex)
        {
        }
    }


}
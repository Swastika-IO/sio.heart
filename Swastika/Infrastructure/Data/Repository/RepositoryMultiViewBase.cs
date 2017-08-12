using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swastika.Common.Helper;
using Swastika.Domain.Core.Interfaces;
using Swastika.Domain.Core.Models;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Swastika.Infrastructure.Data.Repository
{
    /// <summary>
    /// Base Repository
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="Swastika.Extension.Blog.Interfaces.IRepository{TModel, TView}" />
    public abstract class SWRepositoryBase<TModel, TBaseView, TView, TContext>
       where TModel : class
        where TBaseView : SWViewModelBase<TModel, TView>
        where TView : IExpandViewModel<TModel>
        where TContext : DbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SWBaseRepository{TModel, TView, TContext}"/> class.
        /// </summary>
        public SWRepositoryBase()
        {
            RegisterAutoMapper();
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <returns></returns>
        public virtual TContext InitContext()
        {
            Type classType = typeof(TContext);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
            TContext context = (TContext)classConstructor.Invoke(new object[] { });

            return context;
        }

        /// <summary>
        /// Registers the automatic mapper.
        /// </summary>
        public virtual void RegisterAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TModel, TView>();
                cfg.CreateMap<TView, TModel>();
            });
        }

        /// <summary>
        /// Parses the view.
        /// </summary>
        /// <param name="lstModels">The LST models.</param>
        /// <returns></returns>
        public virtual List<TView> ParseView(List<TModel> lstModels)
        {
            List<TView> lstView = new List<TView>();
            foreach (var model in lstModels)
            {
                lstView.Add(ParseView(model));
            }

            return lstView;
        }

        /// <summary>
        /// Parses the view.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual TView ParseView(TModel model)
        {
            Type classType = typeof(TBaseView);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { model.GetType() });
            TBaseView vm = (TBaseView)classConstructor.Invoke(new object[] { model });
            return vm.View;
        }

        /// <summary>
        /// Determines whether the specified entity is exists.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CheckIsExists(TModel entity, TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
        public bool CheckIsExists(System.Func<TModel, bool> predicate, TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
        public virtual RepositoryResponse<TView> CreateModel(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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

                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = ParseView(model)
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        transaction.Rollback();
                    }

                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = default(TView)
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
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
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
        public virtual async Task<RepositoryResponse<TView>> CreateModelAsync(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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

                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = ParseView(model)
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = default(TView)
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

                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView)
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
        public virtual RepositoryResponse<TView> EditModel(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = ParseView(model)
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = default(TView)
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
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
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
        public virtual async Task<RepositoryResponse<TView>> EditModelAsync(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = ParseView(model)
                    };
                }
                else
                {
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = default(TView)
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

                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
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
        /// Gets the view model list asynchronous.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<List<TView>> GetViewModelListAsync()
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TView> lstViewResult = new List<TView>();
                    var lstModel = await context.Set<TModel>().ToListAsync();
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    lstViewResult = ParseView(lstModel);
                    return lstViewResult;
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual List<TView> GetModelList()
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TView> lstViewResult = new List<TView>();
                    var lstModel = context.Set<TModel>().ToList();

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    lstViewResult = ParseView(lstModel);
                    return lstViewResult;
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelList(
            Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>();

                    PaginationModel<TView> result = new PaginationModel<TView>()
                    {
                        TotalItems = query.Count(),
                        PageIndex = pageIndex ?? 0
                    };
                    result.PageSize = pageSize ?? result.TotalItems;

                    if (pageSize.HasValue)
                    {
                        result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                    }

                    // TODO: should we change "direction" to boolean "isDesc" and use if condition instead?
                    switch (direction)
                    {
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelList(
            Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();

                    var query = context.Set<TModel>();
                    PaginationModel<TView> result = new PaginationModel<TView>()
                    {
                        TotalItems = query.Count(),
                        PageIndex = pageIndex ?? 0
                    };
                    result.PageSize = pageSize ?? result.TotalItems;

                    if (pageSize.HasValue)
                    {
                        result.TotalPage = result.TotalItems / pageSize.Value + (result.TotalItems % pageSize.Value > 0 ? 1 : 0);
                    }

                    // TODO: should we change "direction" to boolean "isDesc" and use if condition instead?
                    switch (direction)
                    {
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);
                    result.Items = lstViewResult;

                    return result;
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelList(
            Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>();

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value).ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListAsync(
            Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>();

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query.OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value).ToListAsync();
                            }
                            else
                            {
                                lstModel = await query.OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;

                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<List<TView>> GetModelListAsync()
        {
            using (TContext context = InitContext())
            {
                try
                {
                    var lstModel = await context.Set<TModel>().ToListAsync();
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);

                    return lstViewResult;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListAsync(
            Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>();

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);
                    result.Items = lstViewResult;

                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListAsync(
            Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>();

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual List<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    var lstModel = context.Set<TModel>().Where(predicate).ToList();
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);
                    return lstViewResult;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelListBy(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);
                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelListBy(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual PaginationModel<TView> GetModelListBy(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderByDescending(orderBy)
                                    .ToList();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToList();
                            }
                            else
                            {
                                lstModel = query
                                    .OrderBy(orderBy)
                                    .ToList();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the model list by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<List<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    var lstModel = await context.Set<TModel>().Where(predicate).ToListAsync();
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);
                    return lstViewResult;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListByAsync(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction,
            int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }

                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListByAsync(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);

                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual async Task<PaginationModel<TView>> GetModelListByAsync(
            Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize)
        {
            using (TContext context = InitContext())
            {
                try
                {
                    List<TModel> lstModel = new List<TModel>();
                    var query = context.Set<TModel>().Where(predicate);

                    PaginationModel<TView> result = new PaginationModel<TView>()
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
                        case "desc":
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderByDescending(orderBy)
                                    .ToListAsync();
                            }
                            break;

                        default:
                            if (pageSize.HasValue)
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .Skip(pageIndex.Value * pageSize.Value)
                                    .Take(pageSize.Value)
                                    .ToListAsync();
                            }
                            else
                            {
                                lstModel = await query
                                    .OrderBy(orderBy)
                                    .ToListAsync();
                            }
                            break;
                    }
                    lstModel.ForEach(model => context.Entry(model).State = EntityState.Detached);
                    var lstViewResult = ParseView(lstModel);

                    result.Items = lstViewResult;
                    return result;
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex);
                    return null;
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
        public virtual TView GetSingleModel(Expression<Func<TModel, bool>> predicate)
        {
            using (TContext context = InitContext())
            {
                TModel model = context.Set<TModel>().FirstOrDefault(predicate);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Detached;
                    var viewResult = ParseView(model);

                    return viewResult;
                }
                else
                {
                    return default(TView);
                }
            }
        }

        /// <summary>
        /// Gets the single model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        public virtual async Task<TView> GetSingleModelAsync(Expression<Func<TModel, bool>> predicate)
        {
            using (TContext context = InitContext())
            {
                TModel model = await context.Set<TModel>().FirstOrDefaultAsync(predicate);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Detached;

                    var viewResult = ParseView(model);
                    return viewResult;
                }
                else
                {
                    return default(TView);
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
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
            , TContext _context = null, IDbContextTransaction _transaction = null)
        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
            , TContext _context = null, IDbContextTransaction _transaction = null)

        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
            , TContext _context = null, IDbContextTransaction _transaction = null)

        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
            , TContext _context = null, IDbContextTransaction _transaction = null)

        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
            , TContext _context = null, IDbContextTransaction _transaction = null)

        {
            TContext context = _context ?? InitContext();
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
                    Ex = ex
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
        public virtual RepositoryResponse<TView> SaveModel(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
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
        public virtual Task<RepositoryResponse<TView>> SaveModelAsync(TModel model, bool isSaveSubModels = false
            , TContext _context = null, IDbContextTransaction _transaction = null)
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

        public virtual bool SaveSubModel(TModel model, TContext context, IDbContextTransaction _transaction)
        {
            return false;
        }

        /// <summary>
        /// Saves the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public virtual Task<bool> SaveSubModelAsync(TModel model, TContext context, IDbContextTransaction _transaction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void LogErrorMessage(Exception ex)
        {
        }
    }
    //public class RepositoryResponse<TResult>
    //{
    //    public bool IsSucceed { get; set; }
    //    public TResult Data { get; set; }
    //    public Exception Ex { get; set; }
    //}
}

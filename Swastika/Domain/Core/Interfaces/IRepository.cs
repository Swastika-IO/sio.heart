using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swastika.Common.Helper;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Swastika.Domain.Core.Interfaces {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public interface IRepository<TModel, TView, TContext> where TModel : class where TView : ViewModelBase<TModel, TView>
        where TContext: DbContext

    {

        /// <summary>
        /// Determines whether the specified entity is exists.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is exists; otherwise, <c>false</c>.
        /// </returns>
        bool CheckIsExists(TView entity, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        RepositoryResponse<TView> CreateModel(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Creates the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<RepositoryResponse<TView>> CreateModelAsync(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Edits the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        RepositoryResponse<TView> EditModel(TView view, bool isSaveSubModels, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Edits the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<RepositoryResponse<TView>> EditModelAsync(TView view, bool isSaveSubModels, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Saves the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        RepositoryResponse<TView> SaveModel(TView view, bool isSaveSubModels, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Saves the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<RepositoryResponse<TView>> SaveModelAsync(TView view, bool isSaveSubModels, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        RepositoryResponse<bool> RemoveModel(TModel model, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        RepositoryResponse<bool> RemoveModel(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the list model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        RepositoryResponse<bool> RemoveListModel(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the model asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<RepositoryResponse<bool>> RemoveModelAsync(TModel model, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<RepositoryResponse<bool>> RemoveModelAsync(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Removes the list model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<RepositoryResponse<bool>> RemoveListModelAsync(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);

        /// <summary>
        /// Gets the single model.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        TView GetSingleModel(Expression<Func<TModel, bool>> predicate, bool isGetSubModels);

        /// <summary>
        /// Gets the single model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        Task<TView> GetSingleModelAsync(Expression<Func<TModel, bool>> predicate, bool isGetSubModels);

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        List<TView> GetModelList(bool isGetSubModels);

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        PaginationModel<TView> GetModelList(Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        PaginationModel<TView> GetModelList(Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        PaginationModel<TView> GetModelList(Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list asynchronous.
        /// </summary>
        /// <param name="orderBy">The order by.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

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
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

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
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

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
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list by.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        List<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, bool isGetSubModels);

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
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

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
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

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
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, bool isGetSubModels);

        /// <summary>
        /// Gets the model list by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isGetSubModels">if set to <c>true</c> [is get sub models].</param>
        /// <returns></returns>
        Task<List<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, bool isGetSubModels);
    }

    public interface IRepository<TEntity> : IDisposable where TEntity : class {
        /// <summary>
        /// Adds the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Add(TEntity obj);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity GetById(Guid id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();
        /// <summary>
        /// Updates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Update(TEntity obj);
        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Remove(Guid id);
        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }

    public class RepositoryResponse<TResult>
    {
        public bool IsSucceed { get; set; }
        public TResult Data { get; set; }
        public Exception Ex { get; set; }
    }
}
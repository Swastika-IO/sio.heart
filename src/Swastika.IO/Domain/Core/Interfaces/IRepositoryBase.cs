using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swastika.Common;
using Swastika.Common.Helper;
using Swastika.Domain.Core.ViewModels;
using Swastika.Domain.Core.Models;

namespace Swastika.Domain.Core.Interfaces
{
    public interface IRepositoryBase<TModel, TView, TContext>
        where TModel : class
        where TView : ViewModelBase<TModel, TView>
        where TContext : DbContext
    {
        bool CheckIsExists(Func<TModel, bool> predicate, TContext _context = null, IDbContextTransaction _transaction = null);
        bool CheckIsExists(TView entity, TContext _context = null, IDbContextTransaction _transaction = null);
        RepositoryResponse<TView> CreateModel(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<TView>> CreateModelAsync(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        RepositoryResponse<TView> EditModel(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<TView>> EditModelAsync(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        List<TView> GetModelList(Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelList(Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelList(Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelList(Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<List<TView>> GetModelListAsync(Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListAsync(Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        List<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        PaginationModel<TView> GetModelListBy(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<List<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, DateTime>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, int>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<PaginationModel<TView>> GetModelListByAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, string>> orderBy, string direction, int? pageIndex, int? pageSize, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        TView GetSingleModel(Expression<Func<TModel, bool>> predicate, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<TView> GetSingleModelAsync(Expression<Func<TModel, bool>> predicate, Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        Task<List<TView>> GetViewModelListAsync(Constants.ViewModelType viewType = Constants.ViewModelType.FrontEnd);
        TContext InitContext();
        void LogErrorMessage(Exception ex);
        List<TView> ParseView(List<TModel> lstModels, Constants.ViewModelType viewType);
        TView ParseView(TModel model, Constants.ViewModelType viewType);
        void RegisterAutoMapper();
        RepositoryResponse<bool> RemoveListModel(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<bool>> RemoveListModelAsync(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);
        RepositoryResponse<bool> RemoveModel(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);
        RepositoryResponse<bool> RemoveModel(TModel model, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<bool>> RemoveModelAsync(Expression<Func<TModel, bool>> predicate, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<bool>> RemoveModelAsync(TModel model, TContext _context = null, IDbContextTransaction _transaction = null);
        RepositoryResponse<TView> SaveModel(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        Task<RepositoryResponse<TView>> SaveModelAsync(TView view, bool isSaveSubModels = false, TContext _context = null, IDbContextTransaction _transaction = null);
        bool SaveSubModel(TView view, TContext context, IDbContextTransaction _transaction);
        Task<bool> SaveSubModelAsync(TView view, TContext context, IDbContextTransaction _transaction);
    }
}
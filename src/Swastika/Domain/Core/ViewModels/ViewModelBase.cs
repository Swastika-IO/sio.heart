using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using Swastika.Domain.Data.Repository;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swastika.Domain.Data.ViewModels
{


    public abstract class ViewModelBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : ViewModelBase<TDbContext, TModel, TView> // instance of inherited
    {
        #region Properties
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }
        [JsonIgnore]
        public string Specificulture { get; set; }

        private static DefaultRepository<TDbContext, TModel, TView> _repo;
        [JsonIgnore]
        public bool IsLazyLoad { get; set; } = true;
        [JsonIgnore]
        public bool IsClone { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; } = 1000;
        [JsonIgnore]
        public int PageIndex { get; set; } = 0;
        [JsonIgnore]
        public int Priority { get; set; }
        [JsonIgnore]
        public static DefaultRepository<TDbContext, TModel, TView> Repository
        {
            get
            {
                if (_repo == null)
                {
                    _repo = DefaultRepository<TDbContext, TModel, TView>.Instance;
                }
                return _repo;
            }
            set => _repo = value;
        }

        private IMapper _mapper;

        [JsonIgnore]
        public IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = this.CreateMapper();
                }
                return _mapper;
            }
            set => _mapper = value;
        }

        private IMapper _modelMapper;
        [JsonIgnore]
        public IMapper ModelMapper
        {
            get
            {
                if (_modelMapper == null)
                {
                    _modelMapper = this.CreateModelMapper();
                }
                return _modelMapper;
            }
            set => _modelMapper = value;
        }

        private IMapper CreateModelMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TModel>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TView>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        [JsonIgnore]
        public List<string> Errors = new List<string>();
        [JsonIgnore]
        public Exception Exception { get; set; }

        /// <summary>
        /// The model
        /// </summary>        
        private TModel _model;

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [JsonIgnore]
        public TModel Model
        {
            get
            {
                if (_model == null)
                {
                    Type classType = typeof(TModel);
                    ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
                    _model = (TModel)classConstructor.Invoke(new object[] { });
                }
                return _model;
            }
            set => _model = value;
        }

        [JsonIgnore]
        public bool IsValid = true;
        #endregion

        #region Common
        /// <summary>
        /// Parses the view.
        /// </summary>
        public virtual TView ParseView(bool isExpand = true, TDbContext _context = null, IDbContextTransaction _transaction = null
            )
        {
            //AutoMapper.Mapper.Map<TModel, TView>(Model, (TView)this);
            Mapper.Map<TModel, TView>(Model, (TView)this);
            if (isExpand)
            {
                bool IsRoot = _context == null;
                var context = _context ?? InitContext();
                var transaction = _transaction ?? context.Database.BeginTransaction();
                try
                {
                    ExpandView(context, transaction);
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    Repository.LogErrorMessage(ex);
                    if (IsRoot)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                }
                finally
                {
                    if (IsRoot)
                    {
                        //if current Context is Root
                        context.Dispose();
                    }
                }

            }
            return (TView)this;
        }
        public virtual void ExpandView(TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

        }
        /// <summary>
        /// Parses the model.
        /// </summary>
        public virtual TModel ParseModel()
        {
            //AutoMapper.Mapper.Map<TView, TModel>((TView)this, Model);
            this.Model = InitModel();
            Mapper.Map<TView, TModel>((TView)this, Model);
            return this.Model;
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
        /// Initializes the context.
        /// </summary>
        /// <returns></returns>
        public virtual TModel InitModel()
        {
            Type classType = typeof(TModel);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
            TModel context = (TModel)classConstructor.Invoke(new object[] { });

            return context;
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <returns></returns>
        public virtual TView InitView(TModel model = null, bool isLazyLoad = true, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            Type classType = typeof(TView);
            TView view = default(TView);

            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
            if (model == null && classConstructor != null)
            {
                view = (TView)classConstructor.Invoke(new object[] { });
                return view;
            }
            else
            {
                classConstructor = classType.GetConstructor(new Type[] { typeof(TModel), typeof(bool), typeof(TDbContext), typeof(IDbContextTransaction) });
                if (classConstructor != null)
                {
                    return (TView)classConstructor.Invoke(new object[] { model, isLazyLoad, _context, _transaction });
                }
                else
                {
                    classConstructor = classType.GetConstructor(new Type[] { typeof(TModel), typeof(TDbContext), typeof(IDbContextTransaction) });
                    return (TView)classConstructor.Invoke(new object[] { model, _context, _transaction });
                }

            }
        }

        public virtual void Validate(TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var validateContext = new System.ComponentModel.DataAnnotations.ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            IsValid = Validator.TryValidateObject(this, validateContext, results);
            if (!IsValid)
            {
                Errors.AddRange(results.Select(e => e.ErrorMessage));
            }
        }
        #endregion


        #region Async
        public virtual async Task<RepositoryResponse<bool>> RemoveModelAsync(bool isRemoveRelatedModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                ParseModel();
                if (isRemoveRelatedModels)
                {
                    var removeRelatedResult = await RemoveRelatedModelsAsync((TView)this, context, transaction);
                    if (removeRelatedResult.IsSucceed)
                    {
                        result = await Repository.RemoveModelAsync(Model, context, transaction);
                    }
                    else
                    {
                        result.IsSucceed = result.IsSucceed && removeRelatedResult.IsSucceed;
                        result.Errors.AddRange(removeRelatedResult.Errors);
                        result.Exception = removeRelatedResult.Exception;
                    }
                }
                
                if (result.IsSucceed)
                {
                    result = await Repository.RemoveModelAsync(Model, context, transaction);
                    if (_transaction == null)
                    {
                        transaction.Commit();
                    }

                    return result;
                }
                else
                {
                    if (_transaction == null)
                    {
                        transaction.Rollback();
                    }
                    result.IsSucceed = false;
                    return result;
                }
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
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


        public virtual async Task<RepositoryResponse<TView>> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            bool IsRoot = _context == null;
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<TView> result = new RepositoryResponse<TView>() { IsSucceed = true };
            Validate();
            if (IsValid)
            {
                try
                {
                    ParseModel();
                    result = await Repository.SaveModelAsync((TView)this, _context: context, _transaction: transaction);

                    // Save sub Models
                    if (result.IsSucceed && isSaveSubModels)
                    {
                        var saveResult = await SaveSubModelsAsync(Model, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                        }
                        result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;

                    }

                    // Clone Models
                    if (result.IsSucceed && IsClone && IsRoot)
                    {
                        var cloneCultures = ListSupportedCulture.Where(c => c.Specificulture != Specificulture && c.IsSupported).ToList();
                        var cloneResult = await CloneAsync(cloneCultures, _context: context, _transaction: transaction);
                        if (!cloneResult.IsSucceed)
                        {
                            result.Errors.AddRange(cloneResult.Errors);
                            result.Exception = cloneResult.Exception;
                        }
                        result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                    }


                    //Commit context
                    if (result.IsSucceed)
                    {
                        if (IsRoot)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        result.Data = this as TView;
                        return result;
                    }
                    else
                    {
                        if (IsRoot)
                        {
                            //if current transaction is root transaction
                            transaction.Rollback();
                        }
                        return result;
                    }
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    Repository.LogErrorMessage(ex);
                    if (IsRoot)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    result.IsSucceed = false;
                    result.Exception = ex;
                    return result;
                }
                finally
                {
                    if (IsRoot)
                    {
                        //if current Context is Root
                        context.Dispose();
                    }
                }
            }
            else
            {
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = null,
                    Errors = Errors
                };
            }
        }

        public virtual async Task<RepositoryResponse<List<TView>>> CloneAsync(List<SupportedCulture> cloneCultures
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            bool IsRoot = _context == null;
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
            {
                IsSucceed = true,
                Data = new List<TView>()
            };

            try
            {
                if (cloneCultures != null)
                {


                    foreach (var culture in cloneCultures)
                    {
                        string desSpecificulture = culture.Specificulture;

                        TView view = InitView();
                        view.Model = this.Model;
                        view.ParseView(isExpand: false, _context: context, _transaction: transaction);
                        view.Specificulture = desSpecificulture;

                        bool isExist = Repository.CheckIsExists(view.ParseModel(), _context: context, _transaction: transaction);

                        if (isExist)
                        {
                            result.IsSucceed = true;
                            result.Data.Add(view);
                        }
                        else
                        {
                            var cloneResult = await view.SaveModelAsync(false, context, transaction);
                            if (cloneResult.IsSucceed)
                            {
                                var cloneSubResult = await CloneSubModelsAsync(cloneResult.Data, cloneCultures, context, transaction);
                                if (!cloneSubResult.IsSucceed)
                                {
                                    cloneResult.Errors.AddRange(cloneSubResult.Errors);
                                    cloneResult.Exception = cloneSubResult.Exception;
                                }

                                result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed && cloneSubResult.IsSucceed;
                                result.Data.Add(cloneResult.Data);
                            }
                            else
                            {
                                result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                                result.Errors.AddRange(cloneResult.Errors);
                                result.Exception = cloneResult.Exception;
                            }

                        }


                        if (result.IsSucceed)
                        {

                            if (_transaction == null)
                            {
                                transaction.Commit();
                            }

                        }
                        else
                        {

                            if (_transaction == null)
                            {
                                transaction.Rollback();
                            }
                        }

                    }
                    return result;
                }
                else
                {
                    return result;
                }
            }

            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
            finally
            {
                if (_context == null)
                {
                    _context.Dispose();
                }
            }
        }

#pragma warning disable CS1998 // Override optional
        public virtual async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(TView view, TDbContext _context = null, IDbContextTransaction _transaction = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>());
            return taskSource.Task.Result;
        }

#pragma warning disable CS1998 // Override optional
        public virtual async Task<RepositoryResponse<bool>> SaveSubModelsAsync(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>() { IsSucceed = true });
            return taskSource.Task.Result;
        }

#pragma warning disable CS1998 // Override optional
        public virtual async Task<RepositoryResponse<bool>> CloneSubModelsAsync(TView parent, List<SupportedCulture> cloneCultures, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>() { IsSucceed = true, Data = true });
            return taskSource.Task.Result;
        }

        #endregion

        #region Sync
        public virtual RepositoryResponse<bool> RemoveModel(bool isRemoveRelatedModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                ParseModel();
                if (isRemoveRelatedModels)
                {
                    var removeRelatedResult = RemoveRelatedModels((TView)this, context, transaction);
                    if (removeRelatedResult.IsSucceed)
                    {
                        result = Repository.RemoveModel(Model, context, transaction);
                    }
                    else
                    {
                        result.IsSucceed = result.IsSucceed && removeRelatedResult.IsSucceed;
                        result.Errors.AddRange(removeRelatedResult.Errors);
                        result.Exception = removeRelatedResult.Exception;
                    }
                }

                if (result.IsSucceed)
                {
                    result = Repository.RemoveModel(Model, context, transaction);
                    if (_transaction == null)
                    {
                        transaction.Commit();
                    }

                    return result;
                }
                else
                {
                    if (_transaction == null)
                    {
                        transaction.Rollback();
                    }
                    result.IsSucceed = false;
                    return result;
                }
            }
            // TODO: Add more specific exeption types instead of Exception only
            catch (Exception ex)
            {
                if (_transaction == null)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
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


        public virtual RepositoryResponse<TView> SaveModel(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            bool IsRoot = _context == null;
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<TView> result = new RepositoryResponse<TView>() { IsSucceed = true };
            Validate();
            if (IsValid)
            {
                try
                {
                    ParseModel();
                    result = Repository.SaveModel((TView)this, _context: context, _transaction: transaction);

                    // Save sub Models
                    if (result.IsSucceed && isSaveSubModels)
                    {
                        var saveResult = SaveSubModels(Model, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                        }
                        result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;

                    }

                    // Clone Models
                    if (result.IsSucceed && IsClone && IsRoot)
                    {
                        var cloneCultures = ListSupportedCulture.Where(c => c.Specificulture != Specificulture && c.IsSupported).ToList();
                        var cloneResult = Clone(cloneCultures, _context: context, _transaction: transaction);
                        if (!cloneResult.IsSucceed)
                        {
                            result.Errors.AddRange(cloneResult.Errors);
                            result.Exception = cloneResult.Exception;
                        }
                        result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                    }


                    //Commit context
                    if (result.IsSucceed)
                    {
                        if (IsRoot)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        result.Data = this as TView;
                        return result;
                    }
                    else
                    {
                        if (IsRoot)
                        {
                            //if current transaction is root transaction
                            transaction.Rollback();
                        }
                        return result;
                    }
                }
                // TODO: Add more specific exeption types instead of Exception only
                catch (Exception ex)
                {
                    Repository.LogErrorMessage(ex);
                    if (IsRoot)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    result.IsSucceed = false;
                    result.Exception = ex;
                    return result;
                }
                finally
                {
                    if (IsRoot)
                    {
                        //if current Context is Root
                        context.Dispose();
                    }
                }
            }
            else
            {
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = null,
                    Errors = Errors
                };
            }
        }

        public virtual RepositoryResponse<List<TView>> Clone(List<SupportedCulture> cloneCultures
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            bool IsRoot = _context == null;
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
            {
                IsSucceed = true,
                Data = new List<TView>()
            };

            try
            {
                if (cloneCultures != null)
                {


                    foreach (var culture in cloneCultures)
                    {
                        string desSpecificulture = culture.Specificulture;

                        TView view = InitView();
                        view.Model = this.Model;
                        view.ParseView(isExpand: false, _context: context, _transaction: transaction);
                        view.Specificulture = desSpecificulture;

                        bool isExist = Repository.CheckIsExists(view.ParseModel(), _context: context, _transaction: transaction);

                        if (isExist)
                        {
                            result.IsSucceed = true;
                            result.Data.Add(view);
                        }
                        else
                        {
                            var cloneResult = view.SaveModel(false, context, transaction);
                            if (cloneResult.IsSucceed)
                            {
                                var cloneSubResult = CloneSubModels(cloneResult.Data, cloneCultures, context, transaction);
                                if (!cloneSubResult.IsSucceed)
                                {
                                    cloneResult.Errors.AddRange(cloneSubResult.Errors);
                                    cloneResult.Exception = cloneSubResult.Exception;
                                }

                                result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed && cloneSubResult.IsSucceed;
                                result.Data.Add(cloneResult.Data);
                            }
                            else
                            {
                                result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                                result.Errors.AddRange(cloneResult.Errors);
                                result.Exception = cloneResult.Exception;
                            }

                        }


                        if (result.IsSucceed)
                        {

                            if (_transaction == null)
                            {
                                transaction.Commit();
                            }

                        }
                        else
                        {

                            if (_transaction == null)
                            {
                                transaction.Rollback();
                            }
                        }

                    }
                    return result;
                }
                else
                {
                    return result;
                }
            }

            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
                return result;
            }
            finally
            {
                if (_context == null)
                {
                    _context.Dispose();
                }
            }
        }
        public virtual RepositoryResponse<bool> RemoveRelatedModels(TView view, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return new RepositoryResponse<bool>() { IsSucceed = true };
        }

        public virtual RepositoryResponse<bool> SaveSubModels(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return new RepositoryResponse<bool>() { IsSucceed = true };
        }

        public virtual RepositoryResponse<bool> CloneSubModels(TView parent, List<SupportedCulture> cloneCultures, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return new RepositoryResponse<bool>() { IsSucceed = true };
        }
        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ViewModelBase(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Model = model;
            ParseView(_context: _context, _transaction: _transaction);

        }
        public ViewModelBase(TModel model, bool isLazyLoad, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Model = model;
            IsLazyLoad = isLazyLoad;
            ParseView(isExpand: isLazyLoad, _context: _context, _transaction: _transaction);

        }
        public ViewModelBase()
        {
            this.Model = InitModel();
            ParseView();
        }
        #endregion

    }
}


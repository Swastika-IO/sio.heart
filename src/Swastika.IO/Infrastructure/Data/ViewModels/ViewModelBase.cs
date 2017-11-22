using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using Swastika.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Swastika.Infrastructure.Data.ViewModels
{
   

    public abstract class ViewModelBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : ViewModelBase<TDbContext, TModel, TView> // instance of inherited
    {

        public string Specificulture { get; set; }
        private static DefaultRepository<TDbContext, TModel, TView> _repo;
        public bool IsLazyLoad { get; set; } = true;
        public bool IsClone { get; set; }
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
        public Exception Ex { get; set; }

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

        /// <summary>
        /// Parses the view.
        /// </summary>
        public virtual TView ParseView(TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            //AutoMapper.Mapper.Map<TModel, TView>(Model, (TView)this);
            Mapper.Map<TModel, TView>(Model, (TView)this);
            return (TView)this;
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
                return (TView)classConstructor.Invoke(new object[] { model, isLazyLoad, _context, _transaction });
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

        public virtual async Task<RepositoryResponse<bool>> RemoveModelAsync(bool isRemoveRelatedModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
                ParseModel();
                if (isRemoveRelatedModels)
                {
                    var removeRelatedResult = await RemoveRelatedModelsAsync((TView)this, context, transaction);
                    if (removeRelatedResult.IsSucceed)
                    {
                        result = await Repository.RemoveModelAsync(Model, context, transaction);
                    }
                }
                result = await Repository.RemoveModelAsync(Model, context, transaction);
                if (result.IsSucceed)
                {

                    if (_transaction == null)
                    {
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
                        transaction.Rollback();
                    }

                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false,
                        Errors = result.Errors,
                        Ex = result.Ex
                    };
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
        public virtual async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(TView view, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>());
            return taskSource.Task.Result;
        }

        public virtual RepositoryResponse<bool> RemoveModel(bool isRemoveRelatedModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
                ParseModel();
                if (isRemoveRelatedModels)
                {
                    var removeRelatedResult = RemoveRelatedModels((TView)this, context, transaction);
                    if (removeRelatedResult.IsSucceed)
                    {
                        result = Repository.RemoveModel(Model, context, transaction);
                    }
                }
                result = Repository.RemoveModel(Model, context, transaction);

                if (result.IsSucceed)
                {

                    if (_transaction == null)
                    {
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
                        transaction.Rollback();
                    }

                    return new RepositoryResponse<bool>()
                    {
                        IsSucceed = false,
                        Data = false,
                        Errors = result.Errors,
                        Ex = result.Ex
                    };
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
        public virtual RepositoryResponse<bool> RemoveRelatedModels(TView view, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return new RepositoryResponse<bool>();
        }

        public virtual async Task<RepositoryResponse<TView>> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<TView> result = new RepositoryResponse<TView>() { IsSucceed = true };
            Validate();
            if (IsValid)
            {
                try
                {

                    ParseModel();
                    result = await Repository.SaveModelAsync((TView)this,_context: context,_transaction: transaction);

                    // Save sub Models
                    if (result.IsSucceed && isSaveSubModels)
                    {
                        var saveResult = await SaveSubModelsAsync(Model, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.Errors.AddRange(saveResult.Errors);
                        }
                        result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                        
                    }

                    // Clone Models
                    if (result.IsSucceed && IsClone)
                    {
                        var cloneResult = await CloneAsync(_context: context, _transaction: transaction);
                        if (!cloneResult.IsSucceed)
                        {
                            result.Errors.AddRange(cloneResult.Errors);
                            result.Ex = cloneResult.Ex;
                        }
                        result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                    }


                    //Commit context
                    if (result.IsSucceed)
                    {
                        result.Data = this as TView;
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        return result;
                    }
                    else
                    {
                        if (_transaction == null)
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
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    result.Ex = ex;
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


        public virtual async Task<RepositoryResponse<bool>> SaveSubModelsAsync(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>());
            return taskSource.Task.Result;
        }

        public virtual RepositoryResponse<TView> SaveModel(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
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
                        }
                        result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;

                    }

                    // Clone Models
                    //if (IsClone)
                    //{
                    //    var cloneResult = Clone(_context: context, _transaction: transaction);
                    //    if (!cloneResult.IsSucceed)
                    //    {
                    //        result.Errors.AddRange(cloneResult.Errors);
                    //        result.Ex = cloneResult.Ex;
                    //    }
                    //    result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed;
                    //}


                    //Commit context
                    if (result.IsSucceed)
                    {
                        result.Data = this as TView;
                        if (_transaction == null)
                        {
                            //if current transaction is root transaction
                            transaction.Commit();
                        }
                        return result;
                    }
                    else
                    {
                        if (_transaction == null)
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
                    if (_transaction == null)
                    {
                        //if current transaction is root transaction
                        transaction.Rollback();
                    }
                    result.Ex = ex;
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

        public virtual RepositoryResponse<bool> SaveSubModels(TModel parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return new RepositoryResponse<bool>();
        }

        public virtual async Task<RepositoryResponse<List<TView>>> CloneAsync(TDbContext _context = null, IDbContextTransaction _transaction = null
            , List<SupportedCulture> supportedCultures = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>() { Data = new List<TView>() };
            
            try
            {
                if (supportedCultures==null)
                {
                    supportedCultures = new List<SupportedCulture>();
                }
                foreach (var culture in supportedCultures.Where(c => c.Specificulture != Specificulture
                    && c.IsSupported))
                {
                    string desSpecificulture = culture.Specificulture;

                    TView view = InitView();
                    Mapper.Map(this.Model, view);                    
                    view.Specificulture = desSpecificulture;                  

                    bool isExist = Repository.CheckIsExists(view.ParseModel(), _context: context, _transaction: transaction);

                    if (isExist)
                    {
                        result.IsSucceed = true;
                        result.Data.Add(InitView(view.Model, true, context, transaction));
                    }
                    else
                    {                        
                        var cloneResult= await view.SaveModelAsync(false, context, transaction);
                        if (cloneResult.IsSucceed)
                        {
                            var cloneSubResult = await CloneSubModelsAsync(cloneResult.Data, context, transaction);
                            if (cloneSubResult.IsSucceed)
                            {
                                cloneResult.Errors.AddRange(cloneSubResult.Errors);
                                cloneResult.Ex = cloneSubResult.Ex;
                            }

                            result.IsSucceed = result.IsSucceed && cloneResult.IsSucceed && cloneSubResult.IsSucceed;
                            result.Data.Add(cloneResult.Data);
                        }
                        else
                        {
                            result.Errors.AddRange(cloneResult.Errors);
                            result.Ex = cloneResult.Ex;
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
            catch (Exception ex)
            {
                result.Ex = ex;
                return result;
            }
            finally
            {
                if (_context == null)
                {
                    _context.Dispose();
                }
            }
            //    var taskSource = new TaskCompletionSource<RepositoryResponse<TView>>();
            //taskSource.SetResult(new RepositoryResponse<TView>());
            //return taskSource.Task.Result;
        }

        public virtual async Task<RepositoryResponse<bool>> CloneSubModelsAsync(TView parent, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var taskSource = new TaskCompletionSource<RepositoryResponse<bool>>();
            taskSource.SetResult(new RepositoryResponse<bool>() { IsSucceed = true, Data = true });
            return taskSource.Task.Result;
        }
        

        

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ViewModelBase(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Model = model;            
            ParseView(_context, _transaction);

        }
        public ViewModelBase(TModel model, bool isLazyLoad, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Model = model;
            IsLazyLoad = isLazyLoad;
            ParseView(_context, _transaction);

        }
        public ViewModelBase()
        {

        }
    }
}


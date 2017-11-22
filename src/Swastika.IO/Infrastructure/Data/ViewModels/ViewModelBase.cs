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
            if (model==null &&classConstructor != null)
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
            Validate();
            if (IsValid)
            {
                ParseModel();
                var result = await Repository.SaveModelAsync((TView)this, isSaveSubModels, _context, _transaction);

                return result;
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
            Validate();
            if (IsValid)
            {
                ParseModel();
                var result = Repository.SaveModel((TView)this, isSaveSubModels, _context, _transaction);

                return result;
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

        //public virtual RepositoryResponse<TView> Clone(string desSpecificulture, TDbContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    return new RepositoryResponse<TView>();
        //}

        public virtual async Task<RepositoryResponse<TView>> CloneAsync(Expression<Func<TModel, bool>> predicate, string desSpecificulture, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            RepositoryResponse<TView> result = new RepositoryResponse<TView>() { };

            try
            {
                TModel newModel = InitModel();
                TView view = InitView();
                //         var keyName = context.Model.FindEntityType(typeof(TModel)).FindPrimaryKey().Properties
                //.Select(x => x.Name).ToList();
                //         var entityType = context.Model.GetEntityTypes(typeof(TModel));
                //         var key = entityType.key();

                result = await Repository.GetSingleModelAsync(predicate, context, transaction);

                if (!result.IsSucceed)
                {

                    ModelMapper.Map(this.Model, newModel);
                    view = InitView(newModel, false, context, transaction);
                    view.Specificulture = desSpecificulture;
                    result = await view.SaveModelAsync(false, context, transaction);
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


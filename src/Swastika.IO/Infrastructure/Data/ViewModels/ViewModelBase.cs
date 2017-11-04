using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using Swastika.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Swastika.Infrastructure.Data.ViewModels
{

    public abstract class ViewModelBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : ViewModelBase<TDbContext, TModel, TView> // instance of inherited
    {

        private static DefaultRepository<TDbContext, TModel, TView> _repo;

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
        public virtual void Validate()
        {
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
                        Data = false
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

        public virtual  RepositoryResponse<bool> RemoveModel(bool isRemoveRelatedModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {

            var context = _context ?? InitContext();
            var transaction = _transaction ?? context.Database.BeginTransaction();
            try
            {
                RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };
                ParseModel();
                if (isRemoveRelatedModels)
                {
                    var removeRelatedResult =  RemoveRelatedModels((TView)this, context, transaction);
                    if (removeRelatedResult.IsSucceed)
                    {
                        result =  Repository.RemoveModel(Model, context, transaction);
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
                        Data = false
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
        public virtual  RepositoryResponse<bool> RemoveRelatedModels(TView view, TDbContext _context = null, IDbContextTransaction _transaction = null)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ViewModelBase(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Model = model;
            ParseView(_context, _transaction);

        }
    }
}


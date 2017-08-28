using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Swastika.Domain.Core.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">database model</typeparam>
    /// <typeparam name="TFEView">Output View</typeparam>
    /// <seealso cref="AutoMapper.Profile" />
    public abstract class SWViewModelBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : ExpandViewModelBase<TDbContext, TModel>
    {
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

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TView>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        /// <summary>
        /// The model
        /// </summary>        
        private TModel _model;

        private TView _view;

        //private ICommand _saveCommand;
        //private ICommand _removeCommand;
        //private ICommand _previewCommand;

        //public abstract void Preview();
        //public abstract void RemoveModel();
        //public abstract bool SaveModel();

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
        public TView View
        {
            get
            {
                if (_view == null)
                {
                    Type classType = typeof(TView);
                    ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
                    _view = (TView)classConstructor.Invoke(new object[] { });
                }
                return _view;
            }
            set => _view = value;
        }



        /// <summary>
        /// Parses the view.
        /// </summary>
        public virtual TView ParseView()
        {
            //AutoMapper.Mapper.Map<TModel, TView>(Model, (TView)this);
            Mapper.Map<TModel, TView>(Model, View);
            return View;
        }

        /// <summary>
        /// Parses the model.
        /// </summary>
        public virtual TModel ParseModel()
        {
            //AutoMapper.Mapper.Map<TView, TModel>((TView)this, Model);
            Mapper.Map<TView, TModel>(View, Model);
            return this.Model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SWViewModelBase(TModel model)
        {
            Model = model;
            ParseView();
            View.ExpandView();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SWViewModelBase(TView view)
        {
            View = view;
            ParseModel();
            view.ExpandModel(Model);
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
        public virtual RepositoryResponse<TView> SaveModel(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            try
            {
                View.Validate();
                if (View.IsValid)
                {
                    var result = View.SaveModel(Model, _context, _transaction);
                    if (result && isSaveSubModels)
                    {
                        result = View.SaveSubModels(Model, _context, _transaction);
                    }

                    if (result)
                    {
                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = true,
                            Data = ParseView()
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = false,
                            Data = default(TView)
                        };
                    }

                    if (result)
                    {
                        ParseView();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = result,
                        Data = View
                    };
                }
                else
                {
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = null,
                        Errors = View.errors
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
                };
            }

        }
        public virtual async Task<RepositoryResponse<TView>> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            try
            {
                View.Validate();
                if (View.IsValid)
                {
                    var result = await View.SaveModelAsync(Model, _context, _transaction);
                    if (result && isSaveSubModels)
                    {
                        result = await View.SaveSubModelsAsync(Model, _context, _transaction);
                    }

                    if (result)
                    {
                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = true,
                            Data = ParseView()
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = false,
                            Data = default(TView)
                        };
                    }

                    if (result)
                    {
                        ParseView();
                        View.ExpandView();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = result,
                        Data = View
                    };
                }
                else
                {
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = null,
                        Errors = View.errors
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
                };
            }

        }

        public virtual async Task<RepositoryResponse<TView>> RemoveModel(bool isSaveSubModels = false, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            try
            {
                if (View.IsValid)
                {
                    var result = await View.RemoveModelAsync(Model, _context, _transaction);
                    if (result && isSaveSubModels)
                    {
                        result = await View.SaveSubModelsAsync(Model, _context, _transaction);
                    }

                    if (result)
                    {
                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = true,
                            Data = ParseView()
                        };
                    }
                    else
                    {

                        return new RepositoryResponse<TView>()
                        {
                            IsSucceed = false,
                            Data = default(TView)
                        };
                    }

                    if (result)
                    {
                        ParseView();
                    }
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = result,
                        Data = View
                    };
                }
                else
                {
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = false,
                        Data = null,
                        Errors = View.errors
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                    Data = default(TView),
                    Ex = ex
                };
            }
        }
    }
}

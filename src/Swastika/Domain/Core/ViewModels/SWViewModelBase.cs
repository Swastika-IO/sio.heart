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
    /// <typeparam name="TView">Output View</typeparam>
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

        public virtual IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TView>().ReverseMap());

            var mapper = new Mapper(config);

            return mapper;
        }

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
            Mapper.Map<TModel, TView>(View.Model, View);
            return View;
        }

        /// <summary>
        /// Parses the model.
        /// </summary>
        public virtual TModel ParseModel()
        {
            //AutoMapper.Mapper.Map<TView, TModel>(View, Model);
            Mapper.Map<TView, TModel>(View, View.Model);
            return View.Model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SWViewModelBase(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            View.Model = model;
            ParseView();
            View.ExpandView(_context, _transaction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SWViewModelBase(TView view)
        {
            View = view;
            ParseModel();
            view.ExpandModel(View.Model);
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
            View.Validate();
            if (View.IsValid)
            {
                var result = View.SaveModel(isSaveSubModels, _context, _transaction);

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
        public virtual async Task<RepositoryResponse<TView>> SaveModelAsync(bool isSaveSubModels = false
            , TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            View.Validate();
            if (View.IsValid)
            {
                var result = await View.SaveModelAsync(isSaveSubModels, _context, _transaction);

                if (result)
                {
                    ParseView();
                    View.ExpandView();
                    return new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = View
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
    }
}

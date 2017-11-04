using AutoMapper;
using Newtonsoft.Json;
using Swastika.Common;
using Swastika.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Swastika.Domain.Core.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <seealso cref="AutoMapper.Profile" />
    public abstract class ViewModelBase<TModel, TView> where TModel : class where TView : ViewModelBase<TModel, TView>
    {
        [JsonIgnore]
        public Constants.ViewModelType ViewType { get; set; }
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }
        [JsonIgnore]
        public string DomainName { get; set; }
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

        /// <summary>
        /// The model
        /// </summary>        
        private TModel _model;

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



        /// <summary>
        /// Parses the view.
        /// </summary>
        public virtual TView ParseView()
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
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        public ViewModelBase(string domainName = "/")
        {
            this.DomainName = domainName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ViewModelBase(TModel model, string domainName = "/") {
            this.DomainName = domainName;
        }
    }
}
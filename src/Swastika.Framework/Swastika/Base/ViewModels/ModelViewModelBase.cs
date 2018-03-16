// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Newtonsoft.Json;
using Swastika.Base.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Swastika.Base.ViewModels
{
    /// <summary>
    /// View Model Base
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public abstract class ViewModelBase<TModel, TView> where TModel : class where TView : ViewModelBase<TModel, TView>
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// The model
        /// </summary>
        private TModel _model;

        /// <summary>
        /// The model mapper
        /// </summary>
        private IMapper _modelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        protected ViewModelBase(string domainName = "/")
        {
            this.DomainName = domainName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TModel, TView}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="domainName">Name of the domain.</param>
        protected ViewModelBase(TModel model, string domainName = "/")
        {
            // TODO: Unused parameter 'model'?
            this.DomainName = domainName;
        }

        /// <summary>
        /// Gets or sets the name of the domain.
        /// </summary>
        /// <value>
        /// The name of the domain.
        /// </value>
        [JsonIgnore]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the list supported culture.
        /// </summary>
        /// <value>
        /// The list supported culture.
        /// </value>
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        [JsonIgnore]
        public IMapper Mapper {
            get { return _mapper ?? (_mapper = this.CreateMapper()); }
            set => _mapper = value;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [JsonIgnore]
        public TModel Model {
            get {
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
        /// Gets or sets the model mapper.
        /// </summary>
        /// <value>
        /// The model mapper.
        /// </value>
        [JsonIgnore]
        public IMapper ModelMapper {
            get { return _modelMapper ?? (_modelMapper = this.CreateModelMapper()); }
            set => _modelMapper = value;
        }

        /// <summary>
        /// Parses the model.
        /// </summary>
        /// <returns></returns>
        public virtual TModel ParseModel()
        {
            //AutoMapper.Mapper.Map<TView, TModel>((TView)this, Model);
            Mapper.Map<TView, TModel>((TView)this, Model);
            return this.Model;
        }

        //TODO: Still need?
        //public abstract void Preview();
        //public abstract void RemoveModel();
        //public abstract bool SaveModel();

        /// <summary>
        /// Parses the view.
        /// </summary>
        /// <returns></returns>
        public virtual TView ParseView()
        {
            //AutoMapper.Mapper.Map<TModel, TView>(Model, (TView)this);
            Mapper.Map<TModel, TView>(Model, (TView)this);
            return (TView)this;
        }

        /// <summary>
        /// Creates the mapper.
        /// </summary>
        /// <returns></returns>
        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TView>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        /// <summary>
        /// Creates the model mapper.
        /// </summary>
        /// <returns></returns>
        private IMapper CreateModelMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TModel>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        //TODO: Still need?
        //private ICommand _saveCommand;
        //private ICommand _removeCommand;
        //private ICommand _previewCommand;
    }
}

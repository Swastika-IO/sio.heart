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
    /// <typeparam name="TModel">database model</typeparam>
    /// <typeparam name="TFEView">Output View</typeparam>
    /// <seealso cref="AutoMapper.Profile" />
    public abstract class ViewModelMultiViewBase<TModel, TFEView>
        where TModel : class
        where TFEView : class
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TFEView>().ReverseMap());
            var mapper = new Mapper(config);
            return mapper;
        }

        /// <summary>
        /// The model
        /// </summary>        
        private TModel _model;

        private TFEView _view;

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
        public TFEView View
        {
            get
            {
                if (_view == null)
                {
                    Type classType = typeof(TFEView);
                    ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
                    _view = (TFEView)classConstructor.Invoke(new object[] { });
                }
                return _view;
            }
            set => _view = value;
        }



        /// <summary>
        /// Parses the view.
        /// </summary>
        public virtual TFEView ParseView()
        {
            //AutoMapper.Mapper.Map<TModel, TView>(Model, (TView)this);
            Mapper.Map<TModel, TFEView>(Model, View);
            return View;
        }

        /// <summary>
        /// Parses the model.
        /// </summary>
        public virtual TModel ParseModel()
        {
            //AutoMapper.Mapper.Map<TView, TModel>((TView)this, Model);
            Mapper.Map<TFEView, TModel>(View, Model);
            return this.Model;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Swastika.Domain.Data.Repository;
using Swastika.IO.Domain.Core.Models;
using Swastika.UI.Base;
using System.Collections.Generic;

namespace Swastika.Api.Controllers
{
    public class BaseApiController: Controller
    {
        protected string _domain;
        protected string _lang;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            base.OnActionExecuting(context);
        }
        protected void GetLanguage()
        {

            _lang = RouteData != null && RouteData.Values["culture"] != null
                ? RouteData.Values["culture"].ToString() : "vi-vn";
            ViewBag.culture = _lang;

            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);

            //ViewBag.currentCulture = listCultures.FirstOrDefault(c => c.Specificulture == _lang);
            //ViewBag.cultures = listCultures;
        }

        public BaseApiController()
        {
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            return base.NotFound(value);
        }

        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (ModelStateEntry state in ViewData.ModelState.Values)
            {
                foreach (ModelError error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return base.BadRequest(modelState);
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            return base.BadRequest(error);
        }

        protected IActionResult GetErrorResult(string responseKey, string errorMsg)
        {
            var result = ApiHelper<string>.GetResult(0, string.Empty, responseKey, null);
            return BadRequest(result);
        }

        //protected IActionResult GetResult<TResult>(int status, TResult data, string responseKey, string error, string message)
        //{
        //    var result = ApiHelper<TResult>.GetResult(status, data, responseKey, null);
        //    return Ok(result);
        //}

        protected IActionResult GetSuccessResult<TResult>(TResult data)
        {
            var result = ApiHelper<TResult>.GetResult(1, data, Enums.ResponseKey.OK.ToString(), null);
            return Ok(result);
        }

    }
    public class BaseApiController<TDbContext, TModel> : Controller
        where TDbContext : DbContext
        where TModel : class
    {
        protected string _domain;
        protected string _lang;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            base.OnActionExecuting(context);
        }
        protected void GetLanguage()
        {

            _lang = RouteData != null && RouteData.Values["culture"] != null
                ? RouteData.Values["culture"].ToString() : "vi-vn";
            ViewBag.culture = _lang;

            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);

            //ViewBag.currentCulture = listCultures.FirstOrDefault(c => c.Specificulture == _lang);
            //ViewBag.cultures = listCultures;
        }

        protected readonly DefaultRepository<TDbContext, TModel> _repo;
        public BaseApiController()
        {
            _repo = DefaultRepository<TDbContext, TModel>.Instance;
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            var result = ApiHelper<TModel>.GetResult(0, default(TModel), Enums.ResponseKey.NotFound.ToString(), null);
            return base.NotFound(result);
        }

        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (ModelStateEntry state in ViewData.ModelState.Values)
            {
                foreach (ModelError error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            var result = ApiHelper<TModel>.GetResult(0, default(TModel), Enums.ResponseKey.BadRequest.ToString(), errors);
            return base.BadRequest(result);
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            var result = ApiHelper<TModel>.GetResult(0, default(TModel), Enums.ResponseKey.BadRequest.ToString(), null);
            return base.BadRequest(result);
        }

        protected IActionResult GetErrorResult(string responseKey, string errorMsg)
        {
            var result = ApiHelper<TModel>.GetResult(0, default(TModel), responseKey, null);
            return BadRequest(result);
        }

        //protected IActionResult GetResult<TResult>(int status, TResult data, string responseKey, string error, string message)
        //{
        //    var result = ApiHelper<TResult>.GetResult(status, data, responseKey, null);
        //    return Ok(result);
        //}

        protected IActionResult GetSuccessResult<TResult>(TResult data)
        {
            var result = ApiHelper<TResult>.GetResult(1, data, Enums.ResponseKey.OK.ToString(), null);
            return Ok(result);
        }

    }

    public class BaseApiController<TDbContext, TModel, TView> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TView : Swastika.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
    {
        protected string _lang;
        protected readonly DefaultRepository<TDbContext, TModel, TView> _repo;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            base.OnActionExecuting(context);
        }
        protected void GetLanguage()
        {

            _lang = RouteData != null && RouteData.Values["culture"] != null
                ? RouteData.Values["culture"].ToString() : "vi-vn";
            ViewBag.culture = _lang;
        }


        public BaseApiController()
        {
            _repo = DefaultRepository<TDbContext, TModel, TView>.Instance;
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            var result = ApiHelper<TView>.GetResult(0, default(TView), Enums.ResponseKey.NotFound.ToString(), null);
            return base.NotFound(result);
        }

        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            List<string> errors = new List<string>();
            foreach (ModelStateEntry state in ViewData.ModelState.Values)
            {
                foreach (ModelError error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            var result = ApiHelper<TView>.GetResult(0, default(TView), Enums.ResponseKey.BadRequest.ToString(), errors);
            return base.BadRequest(result);
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            var result = ApiHelper<TView>.GetResult(0, default(TView), Enums.ResponseKey.BadRequest.ToString(), null);
            return base.BadRequest(result);
        }

        protected IActionResult GetErrorResult(string responseKey, string errorMsg)
        {
            var result = ApiHelper<TView>.GetResult(0, default(TView), responseKey, null);
            return BadRequest(result);
        }

        //protected IActionResult GetResult<TResult>(int status, TResult data, string responseKey, string error, string message)
        //{
        //    var result = ApiHelper<TResult>.GetResult(status, data, responseKey, null);
        //    return Ok(result);
        //}

        protected IActionResult GetSuccessResult<TResult>(TResult data)
        {
            var result = ApiHelper<TResult>.GetResult(1, data, Enums.ResponseKey.OK.ToString(), null);
            return Ok(result);
        }
    }
}
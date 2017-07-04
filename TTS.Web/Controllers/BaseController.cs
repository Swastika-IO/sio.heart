using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swastika.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using TTS.Lib.ViewModels;
using TTS.Lib.Repositories;

namespace TTS.Web.Controllers
{
    public class BaseController : Controller
    {
        public static List<CultureViewModel> listCultures;
        protected string _lang;
        protected IHostingEnvironment _env;
        public BaseController(IHostingEnvironment env)
        {
            _env = env;
            //ViewBag.culture = Request != null && Request.Cookies["culture"] != null ? HttpContext.Request.Cookies["culture"] : "en-us";
            listCultures = CultureRepository.GetInstance().GetModelList();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            base.OnActionExecuting(context);
        }
        protected void GetLanguage()
        {
            _lang = RouteData != null && RouteData.Values["culture"] != null 
                ? RouteData.Values["culture"].ToString() : "en-us";
            ViewBag.culture = _lang;
            ViewBag.currentCulture = listCultures.FirstOrDefault(c => c.Specificulture == _lang);
            ViewBag.cultures = listCultures;
        }
        protected async Task<List<string>> UploadListFileAsync(string folderPath)
        {
            List<string> result = new List<string>();
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                string fileName = await UploadFileAsync(file, folderPath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    result.Add(fileName);
                }

            }
            return result;
        }
        protected async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file != null && file.Length > 0)
            {
                string fileName = await Common.UploadFileAsync(System.IO.Path.Combine(_env.WebRootPath, folderPath), file);
                if (!string.IsNullOrEmpty(fileName))
                {
                    string filePath = Path.Combine(folderPath, fileName);
                    return filePath;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
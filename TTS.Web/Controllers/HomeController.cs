using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TTS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string lang;
        public IActionResult Index()
        {
            string lang = RouteData.Values["culture"].ToString();            
            return View();
        }
        public IActionResult TTS()
        {
            return View();
        }
        [Authorize(Policy = "AddEditUser")]
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DI.Models;
using DependencyInjectionDemo.Services;
using DI.Services;
using Microsoft.Extensions.Configuration;

namespace DI.Controllers
{
    public class HomeController : Controller
    {
        private IDataManger dm;

        public HomeController(IDataManger iDataManger)
        {
            this.dm = iDataManger;

        }
        //public IActionResult Index()
        //{
        //    dm.GetMessage("Sameer");
        //    return View();
        //}


        public IActionResult Index([FromServices] Demo demo, [FromServices] IConfiguration config)
        {
            // dm.GetMessage("Sameer");

            demo.GetMessageDemo("Sameer");
            //config.GetValue<string>("Key here");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

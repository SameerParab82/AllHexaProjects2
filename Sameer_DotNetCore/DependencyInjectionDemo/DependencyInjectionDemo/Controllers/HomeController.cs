using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DependencyInjectionDemo.Models;
using DependencyInjectionDemo.Services;

namespace DependencyInjectionDemo.Controllers
{
    public class HomeController : Controller
    {

        private IDataManger dm;


        public HomeController(IDataManger iDataManger)
        {
            this.dm = iDataManger;
        }
        public IActionResult Index()
        {
            ViewBag.Msg = dm.GetMessage("Sameer Parab");
            
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

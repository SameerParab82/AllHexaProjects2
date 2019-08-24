using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirstMvcCore.Models;
using Microsoft.Extensions.Options;

namespace FirstMvcCore.Controllers
{
    public class HomeController : Controller
    {
        AppConfigModel appConfigModel;
        public HomeController(IOptions<AppConfigModel> options)
        {
            this.appConfigModel = options.Value;

        }

        public IActionResult Index()
        {
            // throw new NullReferenceException();

            ViewBag.Company = appConfigModel.Company;
            ViewBag.ProjectName = appConfigModel.ProjectName;
            ViewBag.Address = appConfigModel.Address;
            ViewBag.Course = appConfigModel.Course;
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

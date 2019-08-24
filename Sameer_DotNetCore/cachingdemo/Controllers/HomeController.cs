using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cachingdemo.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http; // For session.SetString()

namespace cachingdemo.Controllers
{
    public class HomeController : Controller
    {

        private IMemoryCache _memCache;
        private IDistributedCache _disCache;

        private StateDataService _sDataService;
        public HomeController(IMemoryCache memCache, IDistributedCache disCache, StateDataService sDataService)
        {

            this._memCache = memCache;
            this._disCache = disCache;
            this._sDataService = sDataService;
        }

        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client, NoStore = false)]
        public IActionResult Index()
        {
            // ViewBag.now = DateTime.Now;
            if (string.IsNullOrEmpty(_memCache.Get<string>("time")))
            {

               // _memCache.Set<string>("time", DateTime.Now.ToString(), TimeSpan.FromSeconds(5));

                _memCache.Set<string>("time", DateTime.Now.ToString(), new MemoryCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromSeconds(5)
                });
            }
              ViewBag.now = _memCache.Get<string>("time");

            if (string.IsNullOrEmpty(_disCache.GetString("disTime")))
            {

                _disCache.SetString("disTime", DateTime.Now.ToString(), new DistributedCacheEntryOptions
                {

                    SlidingExpiration = TimeSpan.FromSeconds(5)
                });
            }
            ViewBag.DisCachenow = _disCache.GetString("disTime");

            HttpContext.Session.SetString("Msg", "Hello I am Session !");

            _sDataService.Data = "This is some session data";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            ViewData["HttpContext"] = "Http Context item " + HttpContext.Items["DbName"];

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Msg = HttpContext.Session.GetString("Msg");
            ViewBag.ServiceData=  _sDataService.Data;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        class DisCacheModel
        {

            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}

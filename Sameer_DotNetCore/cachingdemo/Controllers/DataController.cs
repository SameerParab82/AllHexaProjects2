using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cachingdemo.Controllers
{
    public class DataController : Controller
    {
        public string Index([FromServices] StateDataService sDataService)
        {
            return sDataService.Data;
        }
    }
}
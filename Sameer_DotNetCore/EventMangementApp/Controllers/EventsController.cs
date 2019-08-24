using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventMangementApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventMangementApp.Controllers
{
    [Route("Event")] //Equivalent to RoutePrefix in mVC 5
    public class EventsController : Controller
    {
        private EventDbContext _dbContext;
        public EventsController(EventDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("list", Name = "ListEvents")]
        public IActionResult Index()
        {
            var Model = _dbContext.Events.ToList();
            return View(Model);
        }

        [HttpGet("New", Name = "AddEvent")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("New", Name = "AddEvent")]
        public async Task<IActionResult> CreateAsync(EventInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(" create",model);
            }
            else
            {
                //  _dbContext.Events.Add(model);
                //  _dbContext.SaveChanges();
                await _dbContext.Events.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToRoute("ListEvents");
            }
        }
    }
}
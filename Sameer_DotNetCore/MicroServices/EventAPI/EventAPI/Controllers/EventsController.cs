using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventAPI.Infrastructure;
using EventAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[RoutePrefix("api/[controller]")]
    //[Route("")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private EventDbContext _dbContext;
        public EventsController(EventDbContext db)
        {
            this._dbContext = db;

        }

        //GET/api/myevents
        [HttpGet]
        //[Route("/GetEvents")]
        [ProducesResponseType((int)HttpStatusCode.OK)]// This attribute are read from dll 
        [AllowAnonymous]
        public ActionResult<IEnumerable<EventInfo>> GetEvents()
        {
            return _dbContext.Events.ToList();
        }

        //GET/api/events/5
        [HttpGet("{id}", Name = "GetEventById")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [AllowAnonymous]
        public ActionResult<EventInfo> GetEventById(int id)
        {
            var item = _dbContext.Events.Find(id);
            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

        //POST /api/events
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<ActionResult<EventInfo>> AddEventAsync(EventInfo model)
        {
            TryValidateModel(model);//Validate Model Explicitly
            if (ModelState.IsValid)
            {
                var result = await _dbContext.Events.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                //below are 3 different ways to generate Location URI
                //return Created($"/api/events/{result.Entity.Id}", result.Entity);
                //return CreatedAtAction(nameof(GetEventById), new { id = result.Entity.Id }, result.Entity);
                return CreatedAtRoute("GetEventById", new { id = result.Entity.Id }, result.Entity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
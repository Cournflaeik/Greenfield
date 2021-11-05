using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RubberDuckyEvents.API.Domain;
using RubberDuckyEvents.API.Ports;

namespace RubberDuckyEvents.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger, IDatabase database)
        {
            _database = database;
            _logger = logger;
        }

        //works
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ViewEvent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(string titleStartsWith) =>
            Ok((await _database.GetAllEvents(titleStartsWith))
                .Select(ViewEvent.FromModel).ToList());

        //works
        //Get request for event based on id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var event_ = await _database.GetEventById(id);
                if (event_ != null)
                {
                    return Ok(ViewEvent.FromModel(event_));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(GetEventById)}");
                return BadRequest(ex.Message);
            }
        }

        //Works
        //Get request for event based on ageRange
        [HttpGet("getByAge/{age}")]
        [ProducesResponseType(typeof(IEnumerable<ViewEvent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(int age) =>
            Ok((await _database.GetEventsByAgeRange(age))
                .Select(ViewEvent.FromModel).ToList());

        //Works
        //Delete request for event based on id
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var event_ = await _database.GetEventById(id);
                if (event_ != null)
                {
                    await _database.DeleteEvent(id);
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(DeleteById)}");
                return BadRequest(ex.Message);
            }
        }

        //works
        //Put request for event
        [HttpPut()]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PersistEvent(CreateEvent event_)
        {
            try
            {
                var createdEvent = event_.ToEvent();
                var persistedEvent = await _database.PersistEvent(createdEvent);
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id.ToString() }, ViewEvent.FromModel(persistedEvent));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistEvent)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
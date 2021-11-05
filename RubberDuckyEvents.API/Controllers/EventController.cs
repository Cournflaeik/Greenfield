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

        //Get request for event based on id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetById(int id)
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
                _logger.LogError(ex, $"Got an error for {nameof(GetById)}");
                return BadRequest(ex.Message);
            }
        }

        //Get request for event based on name
        [HttpGet("{MinimumAge}/{MaximumAge}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAgeRange(DateTime minAge, DateTime maxAge)
        {
            try
            {
                var event_ = await _database.GetEventsByAgeRange(minAge, maxAge);
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
                _logger.LogError(ex, $"Got an error for {nameof(GetById)}");
                return BadRequest(ex.Message);
            }
        }

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
                return CreatedAtAction(nameof(GetById), new { id = createdEvent.Id.ToString() }, ViewEvent.FromModel(persistedEvent));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistEvent)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
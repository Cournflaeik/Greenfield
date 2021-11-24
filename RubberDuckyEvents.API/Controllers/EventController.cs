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

        
        [HttpGet("getAllEvents")] // Define what http request it is and what path it is on
        [ProducesResponseType(typeof(IEnumerable<ViewEvent>), StatusCodes.Status200OK)] // Define a response type 200
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Define a response type 204
        public async Task<IActionResult> Get(string titleStartsWith) => // The thing that is supposed to happen when this request is triggered //This is a lambda function
            Ok((await _database.GetAllEvents(titleStartsWith))
                .Select(ViewEvent.FromModel).ToList());

        //Get request for event based on id
        [HttpGet("getEventById/{id}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                var event_ = await _database.GetEventById(id); // Get event from db
                if (event_ != null) // Check if event is not null
                {
                    return Ok(ViewEvent.FromModel(event_)); // If event is found return event in 200 response
                }
                else
                {
                    return NotFound(); // If event is null return 404
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(GetEventById)}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getEventsByAgeRange/{age}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetEventsByAgeRange(int age)
        {
            try
            {
                var event_ = await _database.GetEventsByAgeRange(age);
                if (event_ != null)
                {
                    return Ok(event_);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(GetEventsByAgeRange)}");
                return BadRequest(ex.Message);
            }
        }

        //Delete request for event based on id
        [HttpDelete("removeEventById/{id}")]
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
        [HttpPut("addEditEvent")]
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
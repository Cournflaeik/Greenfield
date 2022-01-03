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

        // Define what http request it is and what path it is on
        [HttpGet("getAllEvents")]
        // Define a response type 200 = The request went great
        [ProducesResponseType(typeof(IEnumerable<ViewEvent>), StatusCodes.Status200OK)]
        // Define a response type 204 = The request went great but there was no content inside
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // => : arrow function = function ...{...} but here you can write it on 1 line
        // task means always async programming, you need this to not obsctruct the other threads
        public async Task<IActionResult> Get(string titleStartsWith) =>
            Ok((await _database.GetAllEvents(titleStartsWith))
                .Select(ViewEvent.FromModel).ToList());

        [HttpGet("getEventById/{id}")]
        // 400 Is a user error, 500 Is a coder error, Every response code starting with 2 is correct https.cat
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById(int id)
        {
            try
            {
                // Get event from database
                var event_ = await _database.GetEventById(id);
                // Check if event is not null
                if (event_ != null)
                {
                    // If event is found return event in 200 response
                    return Ok(ViewEvent.FromModel(event_)); 
                }
                else
                {
                    // If event is null return 404
                    return NotFound(); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(GetEventById)}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getEventParticipantCount/{id}")]
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventParticipantCount(int id)
        {
            try
            {
                var event_ = await _database.GetEventById(id);
                if (event_ != null)
                {
                    var eventCount = await _database.GetEventParticipantCount(id);
                    return Ok(eventCount); 
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


        [HttpGet("getEventsByAgeRange/{age}")]
        // 400 Is a user error, 500 Is a coder error, Every response code starting with 2 is correct https.cat
        [ProducesResponseType(typeof(ViewEvent), StatusCodes.Status200OK)]
        //ProducesResponseType returns the type of responses available
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
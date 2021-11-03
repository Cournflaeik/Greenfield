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
    public class UserController : ControllerBase
    {
        private readonly IDatabase _database;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IDatabase database)
        {
            _database = database;
            _logger = logger;
        }

        //Get request for user based on start of name
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ViewUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(string nameStartsWith) =>
            Ok((await _database.GetAllUsers(nameStartsWith))
                .Select(ViewUser.FromModel).ToList());

        //Get request for user based on ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _database.GetUserById(int.Parse(id));
                if (user != null)
                {
                    return Ok(ViewUser.FromModel(user)); //is niet gelijk ViewUser is hetgeen dat je wilt dat er gezien kan worden (bv. zonder passwoord, enkel mail en naam)
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

        //Get request for user based on name
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var user = await _database.GetUserByName(name);
                if (user != null)
                {
                    return Ok(ViewUser.FromModel(user));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(GetByName)}");
                return BadRequest(ex.Message);
            }
        }

        //Delete request for user based on ID
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var user = await _database.GetUserById(id);
                if (user != null)
                {
                    await _database.DeleteUser(id);
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

        //Put request for user changes
        [HttpPut()]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PersistUser(CreateUser user)
        {
            try
            {
                var createdUser = user.ToUser();
                var persistedUser= await _database.PersistUser(createdUser);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id.ToString() }, ViewUser.FromModel(persistedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistUser)}");
                return BadRequest(ex.Message);
            }
        }

        //Put request for user attendance removal
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAttendance(CreateUser user)
        {
            try
            {
                var createdUser = user.ToUser();
                var persistedUser= await _database.PersistUser(createdUser);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id.ToString() }, ViewUser.FromModel(persistedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistUser)}");
                return BadRequest(ex.Message);
            }
        }

        //Put request for adding user attendance
        [HttpPut("{id}/{eventName}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAttendance(CreateUser user)
        {
            try
            {
                var createdUser = user.ToUser();
                var persistedUser= await _database.PersistUser(createdUser);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id.ToString() }, ViewUser.FromModel(persistedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistUser)}");
                return BadRequest(ex.Message);
            }
        }
    }
}

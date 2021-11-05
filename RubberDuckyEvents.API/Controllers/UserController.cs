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

        //works
        [HttpGet("getAllUsers")]
        [ProducesResponseType(typeof(IEnumerable<ViewUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(string titleStartsWith) =>
            Ok((await _database.GetAllUsers(titleStartsWith))
                .Select(ViewUser.FromModel).ToList());

        //works
        //Get request for user based on ID
        [HttpGet("getUserById/{id}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status200OK)] // 400 fout ligt bij de gebruiker, 500 fout ligt bij de maker, alles wat begint met 2 is juist (bv. 204 = juist)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //ProducesResponseType geeft het type van het antwoord
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _database.GetUserById(id);
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
                _logger.LogError(ex, $"Got an error for {nameof(GetUserById)}");
                return BadRequest(ex.Message);
            }
        }

        //works
        //Delete request for user based on ID
        [HttpDelete("removeUserById/{id}")]
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

        //works
        //Put request for user changes
        [HttpPut("addEditUser/")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PersistUser(CreateUser user)
        {
            try
            {
                var createdUser = user.ToUser();
                var persistedUser= await _database.PersistUser(createdUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id.ToString() }, ViewUser.FromModel(persistedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistUser)}");
                return BadRequest(ex.Message);
            }
        }

        //Works
        //Put request for user attendance removal
        [HttpPut("removeAttendance/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAttendance(int id)
        {
            try
            {
                var user = await _database.GetUserById(id);
                if (user != null)
                {
                    await _database.DeleteUserAttendance(id);
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

        //No worky yet
        //Put request for adding user attendance
        [HttpPut("addAttendance/{id}/{eventId}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAttendance(CreateUser user)
        {
            try
            {
                var createdUser = user.ToUser();
                var persistedUser= await _database.PersistUser(createdUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id.ToString() }, ViewUser.FromModel(persistedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(PersistUser)}");
                return BadRequest(ex.Message);
            }
        }
    }
}

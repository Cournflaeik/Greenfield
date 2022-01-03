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

        [HttpGet("getAllUsers")]
        [ProducesResponseType(typeof(IEnumerable<ViewUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(string titleStartsWith) =>
            Ok((await _database.GetAllUsers(titleStartsWith))
                .Select(ViewUser.FromModel).ToList());

        //Get request for user based on ID
        [HttpGet("getUserById/{id}")]
        // 400 Is a user error, 500 Is a coder error, Every response code starting with 2 is correct https.cat
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status200OK)]
        //ProducesResponseType returns the type of responses available
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        //Delete request for user based on Id
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
                    await _database.DeleteUser(user);
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

        //Put request for user attendance removal
        [HttpDelete("event/{eventId}/attendance/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAttendance(int userId, int eventId)
        {
            try
            {

                if (await _database.UserEventExists(userId, eventId))
                {
                    await _database.DeleteUserAttendance(userId, eventId);
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

        //Put request for adding user attendance
        [HttpPut("event/{eventId}/attendance/{userId}")]
        [ProducesResponseType(typeof(ViewUser), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAttendance(int userId, int eventId)
        {
            try
            {
                var user = await _database.GetUserById(userId);
                var event_ = await _database.GetEventById(eventId);
                if (!await _database.UserEventExists(userId, eventId))
                {
                    if (user != null && event_ != null) 
                    {
                        await _database.AddUserAttendance(userId, eventId);
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                } else {
                    return BadRequest("Record already exists or does event and/or user does not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Got an error for {nameof(AddUserAttendance)}");
                return BadRequest(ex.Message);
            }
        }
    }
}
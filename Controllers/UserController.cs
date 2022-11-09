using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                return Ok(_userService.GetUserById(id));
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            try
            {
                if (User.FindFirst("Role")?.Value == Role.Client.ToString())
                    user.Role = Role.Client;

                return Ok(_userService.CreateUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO user)
        {
            try
            {
                if (User.FindFirst("Role")?.Value == Role.Client.ToString())
                    user.Role = Role.Client;

                bool changePassword = user.NewPassword != "";

                return Ok(_userService.UpdateUser(id, user, changePassword));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                return Ok(_userService.DeleteUser(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("authenticate")]
        public IActionResult AuthenticateUser([FromBody] UserDTO user)
        {
            try
            {
                return Ok(_userService.AuthenicateUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("isAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            return Ok(true);
        }

        [Authorize]
        [HttpGet("isAdminAuthenticated")]
        public IActionResult IsAdminAuthenticated()
        {
            if (User.FindFirst("Role")?.Value == Role.Admin.ToString())
                return Ok(true);

            return BadRequest("User does not have admin privileges");
        }
    }
}
using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService _deviceService;

        public DeviceController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetDevices(int? userId)
        {
            try
            {
                if (userId != null)
                    return Ok(_deviceService.GetDevicesByUserId((int)userId));

                return Ok(_deviceService.GetDevices());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getRandomDeviceId")]
        public IActionResult GetRandomDeviceId()
        {
            try
            {
                return Ok(_deviceService.GetRandomDeviceId());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDeviceById(int id)
        {
            try
            {
                return Ok(_deviceService.GetDeviceById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateDevice([FromBody] DeviceDTO device)
        {
            try
            {
                return Ok(_deviceService.CreateDevice(device));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateDevice(int id, [FromBody] DeviceDTO device)
        {
            try
            {
                return Ok(_deviceService.UpdateDevice(id, device));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteDevice(int id)
        {
            try
            {
                return Ok(_deviceService.DeleteDevice(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("mapping")]
        public IActionResult UserDeviceMapping(int? userId, int deviceId)
        {
            try
            {
                return Ok(_deviceService.UserDeviceMapping(userId, deviceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

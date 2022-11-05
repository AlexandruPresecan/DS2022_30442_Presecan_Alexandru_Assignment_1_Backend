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

        [HttpGet]
        public IActionResult GetDevices()
        {
            return Ok(_deviceService.GetDevices());
        }

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
    }
}

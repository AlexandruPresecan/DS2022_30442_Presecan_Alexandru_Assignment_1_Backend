using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyConsumptionController : ControllerBase
    {
        private readonly EnergyConsumptionService _energyConsumptionService;

        public EnergyConsumptionController(EnergyConsumptionService energyConsumptionService)
        {
            _energyConsumptionService = energyConsumptionService;
        }

        [HttpGet]
        public IActionResult GetEnergyConsumptions(int? deviceId, DateTime? date)
        {
            try
            {
                if (deviceId != null && date != null)
                    return Ok(_energyConsumptionService.GetEnergyConsumptionsByDeviceId((int)deviceId, (DateTime)date));

                if (deviceId != null)
                    return Ok(_energyConsumptionService.GetEnergyConsumptionsByDeviceId((int)deviceId));

                return Ok(_energyConsumptionService.GetEnergyConsumptions());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEnergyConsumptionById(int id)
        {
            try
            {
                return Ok(_energyConsumptionService.GetEnergyConsumptionById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateEnergyConsumption([FromBody] EnergyConsumptionDTO device)
        {
            try
            {
                return Ok(_energyConsumptionService.CreateEnergyConsumption(device));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateEnergyConsumption(int id, [FromBody] EnergyConsumptionDTO device)
        {
            try
            {
                return Ok(_energyConsumptionService.UpdateEnergyConsumption(id, device));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteEnergyConsumption(int id)
        {
            try
            {
                return Ok(_energyConsumptionService.DeleteEnergyConsumption(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

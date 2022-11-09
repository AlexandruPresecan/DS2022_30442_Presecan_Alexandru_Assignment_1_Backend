using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs
{
    public class DeviceDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public double? MaximumHourlyEnergyConsumption { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<EnergyConsumptionDTO>? EnergyConsumptions { get; set; }
        
        public DeviceDTO()
        {

        }

        public DeviceDTO(Device device)
        {
            Id = device.Id;
            Description = device.Description;
            Address = device.Address;
            MaximumHourlyEnergyConsumption = device.MaximumHourlyEnergyConsumption;
            UserId = device.UserId;
            EnergyConsumptions = device.EnergyConsumptions?.Select(energyConsumption => new EnergyConsumptionDTO(energyConsumption));
        }
    }
}

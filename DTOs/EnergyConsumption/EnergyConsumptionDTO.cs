using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs
{
    public class EnergyConsumptionDTO
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double EnergyConsumptionValue { get; set; }
        public int DeviceId { get; set; }

        public EnergyConsumptionDTO()
        {

        }

        public EnergyConsumptionDTO(EnergyConsumption energyConsumption)
        {
            Id = energyConsumption.Id;
            TimeStamp = energyConsumption.TimeStamp;
            EnergyConsumptionValue = energyConsumption.EnergyConsumptionValue;
            DeviceId = energyConsumption.DeviceId;
        }
    }
}

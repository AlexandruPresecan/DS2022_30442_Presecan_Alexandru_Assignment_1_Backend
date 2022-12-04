using DS2022_30442_Presecan_Alexandru_Assignment_1.Data;
using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;
using DS2022_30442_Presecan_Alexandru_Assignment_1_Backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Services
{
    public class EnergyConsumptionService
    {
        private readonly DataContext _db;
        private readonly IHubContext<NotifyHub> _notifyHubContext;

        public EnergyConsumptionService(DataContext db, IHubContext<NotifyHub> notifyHubContext)
        {
            _db = db;
            _notifyHubContext = notifyHubContext;
        }

        public IEnumerable<EnergyConsumptionDTO> GetEnergyConsumptions() => 
            _db.EnergyConsumptions
            .Select(energyConsumption => new EnergyConsumptionDTO(energyConsumption));
        
        public IEnumerable<EnergyConsumptionDTO> GetEnergyConsumptionsByDeviceId(int deviceId) => 
            GetEnergyConsumptions()
            .Where(energyConsumption => energyConsumption.DeviceId == deviceId);

        public IEnumerable<EnergyConsumptionDTO> GetEnergyConsumptionsByDeviceId(int deviceId, DateTime date) => 
            GetEnergyConsumptions()
            .Where(energyConsumption => energyConsumption.DeviceId == deviceId && energyConsumption.TimeStamp?.Date == date.Date)
            .OrderBy(energyConsumptioin => energyConsumptioin.TimeStamp);

        public EnergyConsumptionDTO? GetEnergyConsumptionById(int id) => 
            GetEnergyConsumptions()
            .FirstOrDefault(energyConsumption => energyConsumption.Id == id);

        public double GetCurrentHourlyEnergyConsumption(int deviceId) =>
            GetEnergyConsumptionsByDeviceId(deviceId, DateTime.Now)
            .Where(energyConsumption => energyConsumption.TimeStamp?.Hour == DateTime.Now.Hour)
            .Sum(energyConsumption => energyConsumption.EnergyConsumptionValue);

        public EnergyConsumptionDTO? CreateEnergyConsumption(EnergyConsumptionDTO energyConsumptionDTO)
        {
            Device? device = _db.Devices.FirstOrDefault(device => device.Id == energyConsumptionDTO.DeviceId);

            if (device == null)
                throw new Exception("Device not found");

            EnergyConsumption energyConsumption = new EnergyConsumption()
            {
                TimeStamp = energyConsumptionDTO.TimeStamp,
                EnergyConsumptionValue = energyConsumptionDTO.EnergyConsumptionValue,
                DeviceId = energyConsumptionDTO.DeviceId
            };

            _db.EnergyConsumptions.Add(energyConsumption);
            _db.SaveChanges();

            double currentHourlyEnergyConsumption = GetCurrentHourlyEnergyConsumption(device.Id);

            if (device.UserId != null && currentHourlyEnergyConsumption > device.MaximumHourlyEnergyConsumption)
            {
                DeviceDTO data = new DeviceDTO(device);
                data.CurrentHourlyEnergyConsumption = currentHourlyEnergyConsumption;
                NotifyHub.Notify(_notifyHubContext, device.UserId.ToString(), data);
            }

            return GetEnergyConsumptionById(energyConsumption.Id);
        }

        public EnergyConsumptionDTO? UpdateEnergyConsumption(int id, EnergyConsumptionDTO energyConsumptionDTO)
        {
            EnergyConsumption? energyConsumption = _db.EnergyConsumptions.FirstOrDefault(energyConsumption => energyConsumption.Id == id);

            if (energyConsumption == null)
                throw new Exception("Energy Consumption not found");

            if (_db.Devices.FirstOrDefault(device => device.Id == energyConsumptionDTO.DeviceId) == null)
                throw new Exception("Device not found");

            energyConsumption.TimeStamp = energyConsumptionDTO.TimeStamp;
            energyConsumption.EnergyConsumptionValue = energyConsumptionDTO.EnergyConsumptionValue;
            energyConsumption.DeviceId = energyConsumptionDTO.DeviceId;

            _db.EnergyConsumptions.Update(energyConsumption);
            _db.SaveChanges();

            return GetEnergyConsumptionById(energyConsumption.Id);
        }

        public string DeleteEnergyConsumption(int id)
        {
            EnergyConsumption? energyConsumption = _db.EnergyConsumptions.FirstOrDefault(energyConsumption => energyConsumption.Id == id);

            if (energyConsumption == null)
                throw new Exception("Energy Consumption not found");

            _db.EnergyConsumptions.Remove(energyConsumption);
            _db.SaveChanges();

            return "Energy Consumption deleted";
        }
    }
}

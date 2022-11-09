using DS2022_30442_Presecan_Alexandru_Assignment_1.Data;
using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;
using Microsoft.EntityFrameworkCore;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Services
{
    public class DeviceService
    {
        private readonly DataContext _db;
        private readonly EnergyConsumptionService _energyConsumptionService;

        public DeviceService(DataContext db, EnergyConsumptionService energyConsumptionService)
        {
            _db = db;
            _energyConsumptionService = energyConsumptionService;
        }

        public IEnumerable<DeviceDTO> GetDevices() => 
            _db.Devices
            .Include(device => device.EnergyConsumptions)
            .Select(device => new DeviceDTO(device));

        public IEnumerable<DeviceDTO> GetDevicesByUserId(int userId) => 
            GetDevices()
            .Where(device => device.UserId == userId);

        public DeviceDTO? GetDeviceById(int id) => 
            GetDevices()
            .FirstOrDefault(device => device.Id == id);

        public DeviceDTO? CreateDevice(DeviceDTO deviceDTO)
        {
            Device device = new Device()
            {
                UserId = deviceDTO.UserId,
                Address = deviceDTO.Address,
                Description = deviceDTO.Description,
                MaximumHourlyEnergyConsumption = deviceDTO.MaximumHourlyEnergyConsumption,
            };

            _db.Devices.Add(device);
            _db.SaveChanges();

            return GetDeviceById(device.Id);
        }

        public DeviceDTO? UpdateDevice(int id, DeviceDTO deviceDTO)
        {
            Device? device = _db.Devices.FirstOrDefault(device => device.Id == id);

            if (device == null)
                throw new Exception("Device not found");

            device.UserId = deviceDTO.UserId;
            device.Address = deviceDTO.Address;
            device.Description = deviceDTO.Description;
            device.MaximumHourlyEnergyConsumption = deviceDTO.MaximumHourlyEnergyConsumption;

            _db.Devices.Update(device);
            _db.SaveChanges();

            return GetDeviceById(device.Id);
        }

        public string DeleteDevice(int id)
        {
            Device? device = _db.Devices.Include(device => device.EnergyConsumptions).FirstOrDefault(device => device.Id == id);

            if (device == null)
                throw new Exception("Device not found");

            device.EnergyConsumptions?.ToList().ForEach(energyConsumption => _energyConsumptionService.DeleteEnergyConsumption(energyConsumption.Id));
            _db.Devices.Remove(device);
            _db.SaveChanges();

            return "Device deleted";
        }

        public string UserDeviceMapping(int? userId, int deviceId)
        {
            Device? device = _db.Devices.FirstOrDefault(device => device.Id == deviceId);

            if (device == null)
                throw new Exception("Device not found");

            device.UserId = userId;

            _db.Devices.Update(device);
            _db.SaveChanges();

            return "User-Device mapping added";
        }
    }
}

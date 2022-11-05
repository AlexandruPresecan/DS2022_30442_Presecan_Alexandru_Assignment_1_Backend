using DS2022_30442_Presecan_Alexandru_Assignment_1.Data;
using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;
using Microsoft.EntityFrameworkCore;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Services
{
    public class DeviceService
    {
        private readonly DataContext _db;

        public DeviceService(DataContext db)
        {
            _db = db;
        }

        public IEnumerable<DeviceDTO> GetDevices() => 
            _db.Devices
            .Include(device => device.EnergyConsumptions)
            .Select(device => new DeviceDTO(device));

        public IEnumerable<DeviceDTO> GetDevicesByUserId(int userId) => 
            GetDevices()
            .Where(device => device.UserId == userId);

        public DeviceDTO GetDeviceById(int id) => 
            GetDevices()
            .First(device => device.Id == id);

        public DeviceDTO CreateDevice(DeviceDTO deviceDTO)
        {
            if (_db.Users.First(user => user.Id == deviceDTO.UserId) == null)
                throw new Exception("User not found");

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

        public DeviceDTO UpdateDevice(int id, DeviceDTO deviceDTO)
        {
            Device device = _db.Devices.First(device => device.Id == id);

            if (device == null)
                throw new Exception("Device not found");

            if (_db.Users.First(user => user.Id == deviceDTO.UserId) == null)
                throw new Exception("User not found");

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
            Device device = _db.Devices.First(device => device.Id == id);

            if (device == null)
                throw new Exception("Device not found");

            _db.Devices.Remove(device);
            _db.SaveChanges();

            return "Device deleted";
        }
    }
}

using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs
{
    public class UserDisplayDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public Role Role { get; set; }
        public IEnumerable<DeviceDTO>? Devices { get; set; }

        public UserDisplayDTO()
        {

        }

        public UserDisplayDTO(User user)
        {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
            Role = user.Role;
            Devices = user.Devices?.Select(device => new DeviceDTO(device));
        }
    }
}

using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;
using System.ComponentModel.DataAnnotations;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public Role Role { get; set; }

        public ICollection<Device>? Devices { get; set; }
    }
}

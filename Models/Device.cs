using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public double? MaximumHourlyEnergyConsumption { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<EnergyConsumption>? EnergyConsumptions { get; set; }
    }
}

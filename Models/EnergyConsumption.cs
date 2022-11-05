using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Models
{
    public class EnergyConsumption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime? TimeStamp { get; set; }

        [Required]
        public double EnergyConsumptionValue { get; set; }

        [ForeignKey("Device")]
        public int DeviceId { get; set; }
        public Device? Device { get; set; }
    }
}

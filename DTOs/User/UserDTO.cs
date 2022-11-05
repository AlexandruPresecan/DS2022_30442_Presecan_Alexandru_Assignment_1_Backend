using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? NewPassword { get; set; }
        public Role Role { get; set; }

        public UserDTO()
        {

        }
    }
}

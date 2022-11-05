using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs
{
    public class UserAuthenticationDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public Role Role { get; set; }
        public string? Token { get; set; }

        public UserAuthenticationDTO()
        {

        }

        public UserAuthenticationDTO(User user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
            Role = user.Role;
            Token = token;
        }
    }
}

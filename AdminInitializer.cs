using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Enums;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1_Backend
{
    public class AdminInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            UserService userService = serviceProvider.GetRequiredService<UserService>();

            if (userService.GetUserByName("admin") != null)
                return;

            userService.CreateUser(new UserDTO()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                Password = "admin1234",
                ConfirmPassword = "admin1234",
                Role = Role.Admin
            });
        }
    }
}

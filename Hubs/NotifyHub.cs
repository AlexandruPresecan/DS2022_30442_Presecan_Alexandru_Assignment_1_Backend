using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1_Backend.Hubs
{
    public class NotifyHub : Hub
    {
        private readonly static Dictionary<string, string> userConnectionMapping = new Dictionary<string, string>();

        public static void Notify(IHubContext<NotifyHub> hubContext, string userId, DeviceDTO data)
        {
            if (!userConnectionMapping.ContainsKey(userId))
                return;

            hubContext.Clients.Client(userConnectionMapping[userId]).SendAsync("notify", data);
        }

        [Authorize]
        public override Task OnConnectedAsync()
        {
            userConnectionMapping[Context.User.FindFirst("UserId").Value] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        [Authorize]
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            userConnectionMapping.Remove(Context.User.FindFirst("UserId").Value);
            return base.OnDisconnectedAsync(exception);
        }
    }
}

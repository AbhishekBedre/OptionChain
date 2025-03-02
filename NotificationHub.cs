using Microsoft.AspNetCore.SignalR;

namespace OptionChain
{
    public class NotificationHub:Hub
    {
        public async Task SendMessage(string name)
        {
            await Clients.All.SendAsync("NewStock", name);
        }
    }
}

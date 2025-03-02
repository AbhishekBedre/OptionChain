using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace OptionChain.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NotificationController : ControllerBase
	{
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("notify-intraday")]
		public async Task NotifyIntraday(NotificationType notificationType)
		{
            //send notification to clients
            await _hubContext.Clients.All.SendAsync("NewStock", notificationType);
        }

        [HttpGet("data-update")]
        public async Task NotifyDataUpdate(NotificationType notificationType)
        {
            //send notification to clients
            await _hubContext.Clients.All.SendAsync("DataUpdate", notificationType);
        }
    }
}

public enum NotificationType
{
    DataUpdate,
    Intraday,
    IntradayBlast,
    Daily,
	Weely
}

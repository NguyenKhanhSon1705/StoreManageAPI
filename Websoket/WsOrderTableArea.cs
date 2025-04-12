using Microsoft.AspNetCore.SignalR;

namespace StoreManageAPI.Websoket
{

    public class WsOrderTableArea : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var areaId = httpContext?.Request.Query["areaId"];

            if (!string.IsNullOrEmpty(areaId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Area-{areaId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var areaId = httpContext?.Request.Query["areaId"];

            if (!string.IsNullOrEmpty(areaId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Area-{areaId}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

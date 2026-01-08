using Microsoft.AspNetCore.SignalR;

namespace StoreManageAPI.Websoket
{

    public class WsOrderTableArea : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var areaId = httpContext?.Request.Query["areaId"];
            var tableId = httpContext?.Request.Query["tableId"];

            if (!string.IsNullOrEmpty(areaId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Area-{areaId}");
            }

            // Join table-specific group for dish updates
            if (!string.IsNullOrEmpty(tableId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Table-{tableId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var areaId = httpContext?.Request.Query["areaId"];
            var tableId = httpContext?.Request.Query["tableId"];

            if (!string.IsNullOrEmpty(areaId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Area-{areaId}");
            }

            // Leave table-specific group
            if (!string.IsNullOrEmpty(tableId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Table-{tableId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Join/Leave area groups dynamically
        public async Task JoinArea(int areaId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Area-{areaId}");
        }

        public async Task LeaveArea(int areaId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Area-{areaId}");
        }

        // Allow clients to join a table group dynamically
        public async Task JoinTable(int tableId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Table-{tableId}");
        }

        // Allow clients to leave a table group dynamically
        public async Task LeaveTable(int tableId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Table-{tableId}");
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using PiiSignalRDemo.Utils;

namespace PiiSignalRDemo.Hubs
{
    public class RuleHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var tabId = Context.GetHttpContext().Request.Query["tabId"].ToString();
            var connectionId = Context.ConnectionId;

            ConnectionManager.TabConnections[tabId] = connectionId;

            Console.WriteLine($"Tab: {tabId} → Connection: {connectionId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            var tab = ConnectionManager.TabConnections
                .FirstOrDefault(x => x.Value == connectionId);

            if (!string.IsNullOrEmpty(tab.Key))
            {
                ConnectionManager.TabConnections.Remove(tab.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

using Azure.Core;
using Microsoft.AspNetCore.SignalR;
using PiiSignalRDemo.Models;
using PiiSignalRDemo.Queue;
using PiiSignalRDemo.Utils;
using System.Collections;
using System.Collections.Concurrent;

namespace PiiSignalRDemo.Hubs
{
    public class RuleHub : Hub
    {
        private readonly IQueueService _queue;
        private static ConcurrentDictionary<string, CancellationTokenSource> tokens = new();

        public RuleHub(IQueueService queue)
        {
            _queue = queue;
        }

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
        public async Task ValidatePrompt(string text, string tabId)
        {
            if (tokens.TryGetValue(tabId, out var oldToken))
            {
                oldToken.Cancel();
                oldToken.Dispose();
            }

            var cts = new CancellationTokenSource();
            tokens[tabId] = cts;

            try
            {
                if (!cts.Token.IsCancellationRequested)
                {
                    var request = new PiiRequest
                    {
                        Text = text,
                        TabId = tabId
                    };
                    await _queue.EnqueueAsync(request);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
        }
    }
}

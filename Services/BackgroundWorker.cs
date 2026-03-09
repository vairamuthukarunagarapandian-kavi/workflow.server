using Microsoft.AspNetCore.SignalR;
using PiiSignalRDemo.Hubs;
using PiiSignalRDemo.Queue;
using PiiSignalRDemo.Services;
using PiiSignalRDemo.Utils;

public class BackgroundWorker : BackgroundService
{
    private readonly IQueueService _queue;
    private readonly PiiProcessorService _processor;
    private readonly IHubContext<RuleHub> _hub;

    public BackgroundWorker(
        IQueueService queue,
        PiiProcessorService processor,
        IHubContext<RuleHub> hub)
    {
        _queue = queue;
        _processor = processor;
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var request = await _queue.DequeueAsync(stoppingToken);

            if (request == null)
                continue;

            var result = await _processor.ProcessAsync(request);
            var connectionId = ConnectionManager.TabConnections[request.TabId];
            await _hub.Clients.Client(connectionId).SendAsync("piiResult", result);
        }
    }
}
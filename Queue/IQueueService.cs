using PiiSignalRDemo.Models;

namespace PiiSignalRDemo.Queue
{
    public interface IQueueService
    {
        Task EnqueueAsync(PiiRequest request);
        Task<PiiRequest?> DequeueAsync(CancellationToken token);
    }
}

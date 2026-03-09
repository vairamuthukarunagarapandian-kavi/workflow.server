using PiiSignalRDemo.Models;
using System.Threading.Channels;

namespace PiiSignalRDemo.Queue
{
    public class LocalQueueService : IQueueService
    {
        private readonly Channel<PiiRequest> _queue;

        public LocalQueueService()
        {
            _queue = Channel.CreateUnbounded<PiiRequest>();
        }

        public async Task EnqueueAsync(PiiRequest request)
        {
            await _queue.Writer.WriteAsync(request);
        }

        public async Task<PiiRequest?> DequeueAsync(CancellationToken token)
        {
            return await _queue.Reader.ReadAsync(token);
        }
    }
}

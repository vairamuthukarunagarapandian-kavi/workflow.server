using Azure.Messaging.ServiceBus;
using PiiSignalRDemo.Models;
using System.Text.Json;

namespace PiiSignalRDemo.Queue
{
    public class AzureServiceBusQueueService : IQueueService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly ServiceBusReceiver _receiver;

        public AzureServiceBusQueueService(IConfiguration config)
        {
            var connection = config["ServiceBus:ConnectionString"];
            var queueName = config["ServiceBus:QueueName"];

            _client = new ServiceBusClient(connection);

            _sender = _client.CreateSender(queueName);
            _receiver = _client.CreateReceiver(queueName);
        }

        public async Task EnqueueAsync(PiiRequest request)
        {
            var json = JsonSerializer.Serialize(request);

            var message = new ServiceBusMessage(json);

            await _sender.SendMessageAsync(message);
        }

        public async Task<PiiRequest?> DequeueAsync(CancellationToken token)
        {
            var message = await _receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5), token);

            if (message == null)
                return null;

            var body = message.Body.ToString();

            await _receiver.CompleteMessageAsync(message, token);

            return JsonSerializer.Deserialize<PiiRequest>(body);
        }
    }
}

using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Ats_Demo.Messaging
{
    public class AzureServiceBusPublisher
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public AzureServiceBusPublisher(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureServiceBusConnection");
            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender("employee-updates"); //  Azure Service Bus Topic Name
        }

        public async Task PublishMessageAsync<T>(T messageObject)
        {
            var messageBody = JsonSerializer.Serialize(messageObject);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            await _sender.SendMessageAsync(message);
        }
    }
}

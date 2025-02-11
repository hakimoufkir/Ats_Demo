using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ats_Demo.Repositories.EmployeeRepo;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ats_Demo.Entities;

namespace Ats_Demo.Messaging
{
    public class AzureServiceBusConsumer
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IServiceScopeFactory _scopeFactory;

        public AzureServiceBusConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            var connectionString = configuration.GetConnectionString("AzureServiceBusConnection");
            var client = new ServiceBusClient(connectionString);
            _processor = client.CreateProcessor("employee-updates", "employee-updates-sub", new ServiceBusProcessorOptions());

            _scopeFactory = scopeFactory;
        }

        public async Task StartProcessingAsync()
        {
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            try
            {
                var body = Encoding.UTF8.GetString(args.Message.Body);
                var employee = JsonSerializer.Deserialize<Employee>(body);

                if (employee == null) return;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var _readRepository = scope.ServiceProvider.GetRequiredService<IEmployeeReadRepository>();

                    if (args.Message.ApplicationProperties.TryGetValue("Action", out var action) && action?.ToString() == "Delete")
                    {
                        await _readRepository.DeleteEmployeeAsync(employee.Id);
                    }
                    else
                    {
                        // 🛠️ Use Upsert to prevent duplicate insert errors (E11000)
                        var existingEmployee = await _readRepository.GetByIdAsync(employee.Id);
                        if (existingEmployee == null)
                        {
                            await _readRepository.InsertEmployeeAsync(employee);
                        }
                        else
                        {
                            await _readRepository.UpdateEmployeeAsync(employee);
                        }
                    }
                }

                // ✅ Acknowledge message so it's not reprocessed
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await args.AbandonMessageAsync(args.Message); // 👈 Prevents infinite retry loop
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an error: {args.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}

using Ats_Demo.Infrastructure;
using Ats_Demo.Application;
using Ats_Demo.Middlewares;
using Ats_Demo.Application.Services;
using Ats_Demo.Infrastructure.Messaging;

namespace Ats_Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            


            // Register services for each layer
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddPresentation();

            var app = builder.Build();

            // Allow CORS
            app.UseCors("AllowOrigin");

            // Run MongoDB Migration on Startup
            using (var scope = app.Services.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<MongoDbMigrationService>();
                await migrationService.MigrateEmployeesToMongoDbAsync();
            }

            // Start Azure Service Bus Consumer
            using (var scope = app.Services.CreateScope())
            {
                var serviceBusConsumer = scope.ServiceProvider.GetRequiredService<AzureServiceBusConsumer>();
                await serviceBusConsumer.StartProcessingAsync();
            }

            // Apply Global Exception Handling
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Enable Swagger only in Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

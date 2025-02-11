using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Application.Services;
using Ats_Demo.Infrastructure.Data;
using Ats_Demo.Infrastructure.Messaging;
using Ats_Demo.Infrastructure.Repositories.EmployeeRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Ats_Demo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            // Register MongoDB Client
            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration.GetConnectionString("MongoDbConnection")));

            services.AddSingleton<IMongoDatabase>(s =>
                s.GetRequiredService<IMongoClient>().GetDatabase(configuration["MongoDb:DatabaseName"]));

            // Register Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["ConnectionStrings:RedisConnection"];
                options.InstanceName = "AtsDemo_";
            });

            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            // Repositories
            services.AddScoped<IEmployeeWriteRepository, EmployeeWriteRepository>(); // SQL Server
            services.AddScoped<IEmployeeReadRepository, EmployeeReadRepository>();   // MongoDB

            // Messaging
            services.AddSingleton<AzureServiceBusPublisher>();
            services.AddScoped<AzureServiceBusConsumer>();

            // MongoDB Migration Service
            services.AddScoped<MongoDbMigrationService>();

            // Register Redis Cache Service
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            


            return services;
        }
    }
}

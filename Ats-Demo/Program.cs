using Ats_Demo.Data;
using Ats_Demo.Middlewares;
using Ats_Demo.Repositories.EmployeeRepo;
using Ats_Demo.Services;
using Ats_Demo.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ats_Demo.Messaging;

namespace Ats_Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            // Register MongoDB Client
            builder.Services.AddSingleton<IMongoClient>(s =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection")));

            builder.Services.AddSingleton<IMongoDatabase>(s =>
                s.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration["MongoDb:DatabaseName"]));

            // Register Redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["ConnectionStrings:RedisConnection"];
                options.InstanceName = "AtsDemo_";
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Repositories
            builder.Services.AddScoped<IEmployeeWriteRepository, EmployeeWriteRepository>(); // SQL Server
            builder.Services.AddScoped<IEmployeeReadRepository, EmployeeReadRepository>();   // MongoDB

            // Services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddSingleton<RedisCacheService>();

            // Azure Service Bus
            builder.Services.AddSingleton<AzureServiceBusPublisher>();
            builder.Services.AddScoped<AzureServiceBusConsumer>();

            // MongoDB Migration Service
            builder.Services.AddScoped<MongoDbMigrationService>();

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeeDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetEmployeeByIdQueryValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            // MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // AutoMapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Run migration on startup
            using (var scope = app.Services.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<MongoDbMigrationService>();
                await migrationService.MigrateEmployeesToMongoDbAsync();
            }

            // Start Azure Service Bus Consumer (Real-time Sync SQL ? MongoDB)
            using (var scope = app.Services.CreateScope())
            {
                var serviceBusConsumer = scope.ServiceProvider.GetRequiredService<AzureServiceBusConsumer>();
                await serviceBusConsumer.StartProcessingAsync();
            }

            // Apply global exception handling first
            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
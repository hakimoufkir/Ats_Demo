
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

namespace Ats_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            // Register Redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["ConnectionStrings:RedisConnection"];
                options.InstanceName = "AtsDemo_";
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Repos
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            // services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddSingleton<RedisCacheService>();


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

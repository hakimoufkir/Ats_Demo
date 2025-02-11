using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Ats_Demo
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ats_Demo API", Version = "v1" });
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", policy =>
                    policy.WithOrigins("http://localhost:54883", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            return services;
        }
    }
}

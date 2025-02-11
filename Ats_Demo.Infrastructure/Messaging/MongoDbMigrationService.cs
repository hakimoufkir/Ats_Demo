using Ats_Demo.Application.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ats_Demo.Infrastructure.Messaging
{
    public class MongoDbMigrationService
    {
        private readonly IEmployeeWriteRepository _sqlRepository;
        private readonly IEmployeeReadRepository _mongoRepository;
        private readonly ILogger<MongoDbMigrationService> _logger;

        public MongoDbMigrationService(IEmployeeWriteRepository sqlRepository, IEmployeeReadRepository mongoRepository, ILogger<MongoDbMigrationService> logger)
        {
            _sqlRepository = sqlRepository;
            _mongoRepository = mongoRepository;
            _logger = logger;
        }

        public async Task MigrateEmployeesToMongoDbAsync()
        {
            _logger.LogInformation("Starting migration from SQL Server to MongoDB...");

            try
            {
                var employees = await _sqlRepository.GetAllAsync(null); // Get all employees from SQL Server
                if (employees is null || employees.Count == 0)
                {
                    _logger.LogWarning("No employees found in SQL Server to migrate.");
                    return;
                }

                foreach (var employee in employees)
                {
                    var existingEmployee = await _mongoRepository.GetByIdAsync(employee.Id);

                    if (existingEmployee == null) // Avoid duplicate inserts
                    {
                        await _mongoRepository.InsertEmployeeAsync(employee);
                        _logger.LogInformation($"Migrated Employee ID: {employee.Id} to MongoDB.");
                    }
                    else
                    {
                        _logger.LogWarning($"Employee ID: {employee.Id} already exists in MongoDB, skipping...");
                    }
                }

                _logger.LogInformation("Migration completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during migration: {ex.Message}");
            }
        }
    }
}

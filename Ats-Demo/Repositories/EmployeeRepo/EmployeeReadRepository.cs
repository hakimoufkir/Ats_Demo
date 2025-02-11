using Ats_Demo.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ats_Demo.Repositories.EmployeeRepo
{
    public class EmployeeReadRepository : IEmployeeReadRepository
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeReadRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);
            var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _employees = database.GetCollection<Employee>(configuration["MongoDb:CollectionName"]);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employees.Find(emp => true).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _employees.Find(emp => emp.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertEmployeeAsync(Employee employee)
        {
            var filter = Builders<Employee>.Filter.Eq(e => e.Id, employee.Id);

            await _employees.ReplaceOneAsync(
                filter,
                employee,
                new ReplaceOptions { IsUpsert = true } //  ensures it updates if exists, inserts if not also No more duplicate key errors
            );
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _employees.ReplaceOneAsync(emp => emp.Id == employee.Id, employee);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            await _employees.DeleteOneAsync(emp => emp.Id == id);
        }
    }
}

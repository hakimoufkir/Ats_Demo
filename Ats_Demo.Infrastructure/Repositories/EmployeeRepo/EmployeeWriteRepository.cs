using Ats_Demo.Application.IRepositories;
using Ats_Demo.Domain.Entities;
using Ats_Demo.Infrastructure.Data;
using Ats_Demo.Infrastructure.GenericRepo;
using Ats_Demo.Infrastructure.Messaging;
using System.Threading.Tasks;

namespace Ats_Demo.Infrastructure.Repositories.EmployeeRepo
{
    public class EmployeeWriteRepository : GenericRepository<Employee>, IEmployeeWriteRepository
    {
        private readonly AzureServiceBusPublisher _serviceBusPublisher;

        public EmployeeWriteRepository(ApplicationDbContext db, AzureServiceBusPublisher serviceBusPublisher)
            : base(db)
        {
            _serviceBusPublisher = serviceBusPublisher;
        }

        public override async Task CreateAsync(Employee employee)
        {
            await base.CreateAsync(employee);
            await _serviceBusPublisher.PublishMessageAsync(employee);
        }

        public override async Task UpdateAsync(Employee employee)
        {
            await base.UpdateAsync(employee);
            await _serviceBusPublisher.PublishMessageAsync(employee);
        }

        public override async Task RemoveAsync(Employee employee)
        {
            await base.RemoveAsync(employee);
            await _serviceBusPublisher.PublishMessageAsync(new { employee.Id, Action = "Delete" });
        }
    }
}

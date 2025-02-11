using Ats_Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ats_Demo.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "John Doe",
                    Position = "Software Engineer",
                    Office = "New York",
                    Age = 30,
                    Salary = 60000,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Smith",
                    Position = "Project Manager",
                    Office = "Los Angeles",
                    Age = 35,
                    Salary = 75000,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Michael Johnson",
                    Position = "UI/UX Designer",
                    Office = "San Francisco",
                    Age = 28,
                    Salary = 55000,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }
            );
        }
    }
}

using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Infrastructure.Data;
using Ats_Demo.Infrastructure.Repositories.EmployeeRepo;
using Microsoft.EntityFrameworkCore;

namespace Ats_Demo.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{   
    private readonly ApplicationDbContext _dbContext;
    public IEmployeeWriteRepository EmployeeWriteRepository { get; }
    public IEmployeeReadRepository EmployeeReadRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext, IEmployeeWriteRepository employeeWriteRepository, IEmployeeReadRepository employeeReadRepository)
    {
        _dbContext = dbContext;
        EmployeeWriteRepository = employeeWriteRepository;
        EmployeeReadRepository = employeeReadRepository;
    }
    public void Commit()
    {
        _dbContext.SaveChanges();
    }
    public async Task CommitAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();

        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException != null ? $" Inner exception: {ex.InnerException.Message}" : string.Empty;
            throw new ApplicationException($"An error occurred while saving changes to the database.{innerException}", ex);

        }
    }

    public void Rollback()
    {
        _dbContext.SaveChanges();
    }

    public async Task RollbackAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
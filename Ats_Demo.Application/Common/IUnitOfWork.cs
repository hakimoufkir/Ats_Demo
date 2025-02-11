using Ats_Demo.Application.IRepositories;

namespace Ats_Demo.Application.IUnitOfWork;

public interface IUnitOfWork
{
    IEmployeeWriteRepository EmployeeWriteRepository { get; }
    IEmployeeReadRepository EmployeeReadRepository { get; }
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}
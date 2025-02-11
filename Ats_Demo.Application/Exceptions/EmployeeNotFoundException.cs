namespace Ats_Demo.Application.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(Guid id) : base($"Employee with id :{id} not found !")
        {
        }
    }
}

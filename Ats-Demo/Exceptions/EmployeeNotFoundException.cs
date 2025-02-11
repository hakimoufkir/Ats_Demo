namespace Ats_Demo.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(Guid id) : base($"Employee with id :{id} not found !")
        {
        }
    }
}  

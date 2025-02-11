namespace Ats_Demo.Application.Exceptions
{
    public class EmployeeUpdateException : Exception
    {
        public EmployeeUpdateException(string message) : base(message)
        {

        }
        public EmployeeUpdateException() : base("An error occurred while updating the employee.")
        {
        }

    }
}

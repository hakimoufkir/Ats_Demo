namespace Ats_Demo.Exceptions
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

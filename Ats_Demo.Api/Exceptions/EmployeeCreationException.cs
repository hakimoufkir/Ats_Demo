namespace Ats_Demo.Exceptions
{
    public class EmployeeCreationException : Exception
    {
        public EmployeeCreationException() : base("An error occurred while creating the customer.") { }
        public EmployeeCreationException(string message) : base(message) { }
        public EmployeeCreationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

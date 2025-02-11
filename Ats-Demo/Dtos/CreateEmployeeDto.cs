using System.ComponentModel.DataAnnotations.Schema;

namespace Ats_Demo.Dtos
{
    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public int Age { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}

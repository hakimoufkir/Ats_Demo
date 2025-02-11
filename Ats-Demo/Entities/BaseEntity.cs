using System.ComponentModel.DataAnnotations;

namespace Ats_Demo.Entities
{
    
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
    }
    
}

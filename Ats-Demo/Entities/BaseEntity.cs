using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Ats_Demo.Entities
{
    
    public abstract class BaseEntity
    {
        [Key]
        [BsonId]
        [BsonRepresentation(BsonType.String)]   
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
    }
    
}

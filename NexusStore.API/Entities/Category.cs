
using System.ComponentModel.DataAnnotations;

namespace NexusStore.API.Entities
{
    public class Category : IAuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<Product>? Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
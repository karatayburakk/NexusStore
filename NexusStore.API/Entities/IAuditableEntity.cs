using System.ComponentModel.DataAnnotations;

namespace NexusStore.API.Entities
{
  public interface IAuditableEntity
  {
    public int Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
  }
}

using System.ComponentModel.DataAnnotations;

namespace NexusStore.API.Entities
{
  public interface IBaseEntity
  {
    public int Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
  }
}


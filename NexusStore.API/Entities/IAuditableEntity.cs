namespace NexusStore.API.Entities
{
  public interface IAuditableEntity
  {
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
  }
}


namespace NexusStore.API.Entities
{
  public class Product
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public Category? Category { get; set; }
    public required decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Description { get; set; }
  }
}

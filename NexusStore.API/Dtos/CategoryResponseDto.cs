namespace NexusStore.API.Dtos
{
  public class CategoryResponseDto
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
  }
}
using System.Text.Json;

namespace NexusStore.API.Utilities
{
  public static class JsonOptions
  {
    public static readonly JsonSerializerOptions Options = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true,
    };
  }
}
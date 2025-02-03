using System.Security.Cryptography;

namespace NexusStore.API.Utilities
{
  public class KeyGenerator
  {
    public static string GenerateKey()
    {
      using var hmac = new HMACSHA256();
      return Convert.ToBase64String(hmac.Key);
    }
  }
}
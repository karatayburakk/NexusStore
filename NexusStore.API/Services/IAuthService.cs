using NexusStore.API.Dtos;

namespace NexusStore.API.Services
{
  public interface IAuthService
  {
    Task<string?> LoginAsync(LoginDto loginDto);
    Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto);
  }
}

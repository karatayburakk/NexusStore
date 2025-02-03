using NexusStore.API.Entities;
using NexusStore.API.Repositories;
using AutoMapper;
using NexusStore.API.Dtos;
using Microsoft.Extensions.Configuration;
using Hangfire;

namespace NexusStore.API.Services
{
  public class AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration) : IAuthService
  {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
      var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        return null;

      // Generate JWT token (implementation not shown)
      var token = GenerateJwtToken(user);
      return token;
    }

    public async Task<UserResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
      var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
      if (existingUser != null) return null;

      var user = _mapper.Map<User>(registerDto);
      user.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

      var createdUser = await _userRepository.CreateUserAsync(user);

      BackgroundJob.Enqueue<IEmailService>(emailService => emailService.SendEmailAsync(
      createdUser.Email,
      "Welcome to NexusStore",
      "Thank you for registering!"));

      return _mapper.Map<UserResponseDto>(createdUser);
    }


    private string GenerateJwtToken(User user)
    {
      var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
      var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(user));
      if (jwtKey.Length < 32)
      {
        throw new ArgumentException("JWT key must be at least 256 bits (32 characters) long.");
      }
      var key = System.Text.Encoding.ASCII.GetBytes(jwtKey);
      var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
      {
        Subject = new System.Security.Claims.ClaimsIdentity(
        [
          new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
          new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email)
        ]),
        Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:ExpireDays"] ?? throw new ArgumentNullException(nameof(user)))),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
          new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
          Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}

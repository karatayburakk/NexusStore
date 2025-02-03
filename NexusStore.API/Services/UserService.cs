using NexusStore.API.Entities;
using NexusStore.API.Repositories;
using AutoMapper;
using NexusStore.API.Dtos;

namespace NexusStore.API.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException("User not found.");
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            var createdUser = await _userRepository.CreateUserAsync(user);
            return _mapper.Map<UserResponseDto>(createdUser);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException("User not found.");

            _mapper.Map(updateUserDto, existingUser);

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            return _mapper.Map<UserResponseDto>(updatedUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException("User not found.");

            await _userRepository.DeleteUserAsync(existingUser);
        }
    }
}
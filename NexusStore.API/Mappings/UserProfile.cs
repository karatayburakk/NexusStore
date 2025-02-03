using AutoMapper;
using NexusStore.API.Entities;
using NexusStore.API.Dtos;

namespace NexusStore.API.Mappings
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<CreateUserDto, User>();
      CreateMap<User, UserResponseDto>();
      CreateMap<UpdateUserDto, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
      CreateMap<RegisterDto, User>();
    }
  }
}
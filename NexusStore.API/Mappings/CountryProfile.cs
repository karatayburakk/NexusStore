using AutoMapper;
using NexusStore.API.Entities;
using NexusStore.API.Dtos;

namespace NexusStore.API.Mappings
{
  public class CountryProfile : Profile
  {
    public CountryProfile()
    {
      CreateMap<Country, CountryResponseDto>();
    }
  }
}
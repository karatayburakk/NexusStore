using NexusStore.API.Dtos;

namespace NexusStore.API.Services
{
  public interface IDataService
  {
    Task<IEnumerable<CountryResponseDto>> GetCountriesAsync();
  }
}

using NexusStore.API.Entities;
namespace NexusStore.API.Repositories
{
  public interface ICountryRepository
  {
    Task<IEnumerable<Country>> GetAllCountriesAsync();
  }
}

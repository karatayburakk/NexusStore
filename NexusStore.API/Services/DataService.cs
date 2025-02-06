using NexusStore.API.Entities;
using NexusStore.API.Repositories;
using AutoMapper;
using NexusStore.API.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Enyim.Caching;

namespace NexusStore.API.Services
{
  public class DataService(ICountryRepository countryRepository, IMapper mapper, IDistributedCache cache, IMemcachedClient memcachedClient) : IDataService
  {
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IDistributedCache _cache = cache;

    private readonly IMemcachedClient _memcachedClient = memcachedClient;

    public async Task<IEnumerable<CountryResponseDto>> GetCountriesAsync()
    {
      var cacheKey = "countries"; // This can be a configuration value and not hard-coded, but for simplicity, we'll hard-code it here
      var cacheSeconds = 600; // This can be a configuration value and not hard-coded, but for simplicity, we'll hard-code it here

      try
      {
        var countries = await _memcachedClient.GetValueOrCreateAsync(cacheKey, cacheSeconds, async () =>
        {
          var countries = await _countryRepository.GetAllCountriesAsync();
          return _mapper.Map<IEnumerable<CountryResponseDto>>(countries);
        });
        return countries;
      }
      catch (Exception)
      {
        // fallback to database if caching fails
        var countries = await _countryRepository.GetAllCountriesAsync();
        return _mapper.Map<IEnumerable<CountryResponseDto>>(countries);
      }
    }
  }
}

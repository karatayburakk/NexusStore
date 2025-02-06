using Microsoft.EntityFrameworkCore;
using NexusStore.API.Data;
using NexusStore.API.Entities;

namespace NexusStore.API.Repositories
{
  public class CountryRepository(NexusDbContext context) : ICountryRepository
  {
    private readonly NexusDbContext _context = context;

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
      return await _context.Countries.ToListAsync();
    }
  }
}

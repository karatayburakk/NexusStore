using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusStore.API.Services;

namespace NexusStore.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/[controller]")]
  public class DataController(IDataService dataService) : ControllerBase
  {
    private readonly IDataService _dataService = dataService;

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
      var countries = await _dataService.GetCountriesAsync();
      return Ok(countries);
    }
  }

}
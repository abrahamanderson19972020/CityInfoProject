using CityInfoAPI.Data;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController:ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> GetCities()
        {
            return Ok(CitiesDataStore.GetAllCities());
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCityById(int id)
        {
            List<CityDto> cities = CitiesDataStore.GetAllCities();
            var city = cities.FirstOrDefault(c=> c.Id == id);
            if (city == null)
            {
                return NotFound($"No city found by id = {id}");
            }
            return Ok(city);
        }
    }
}

using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Models.DatabaseSessionConnection;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController:ControllerBase
    {
        private readonly CloudMailManager _mailService;
        private readonly ICityRepository _cityRepositoryManager;
        private readonly IMapper _mapper;

        public CitiesController(CloudMailManager mailService, ICityRepository cityRepositoryManager, IMapper mapper)
        {
            _mailService = mailService;
            _cityRepositoryManager = cityRepositoryManager; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            var cities = await _cityRepositoryManager.GetCitiesAsync();
            _mailService.SendMail("sender@islam.com", "pinar-naim@islam.com","All Cities are listed");
            return Ok(_mapper.Map<IEnumerable<CityDto>>(cities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCityById(int id)
        {
            var city = await _cityRepositoryManager.GetCityByIdAsync(id);
            if (city == null)
            {
                return NotFound($"No city found by id = {id}");
            }
            return Ok(city);
        }
    }
}

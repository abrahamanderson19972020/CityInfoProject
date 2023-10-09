using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Models.DatabaseSessionConnection;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController:ControllerBase
    {
        private readonly CloudMailManager _mailService;
        private readonly ICityRepository _cityRepositoryManager;
        private readonly IMapper _mapper;
        const int MaxPageSize = 10;

        public CitiesController(CloudMailManager mailService, ICityRepository cityRepositoryManager, IMapper mapper)
        {
            _mailService = mailService;
            _cityRepositoryManager = cityRepositoryManager; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(string? name, string? searchString, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }
            var (cities, paginationMetaData) = await _cityRepositoryManager.GetCitiesAsync(name, searchString, pageNumber, pageSize);
            //Here we add pagination meta data to the response heaer
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
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

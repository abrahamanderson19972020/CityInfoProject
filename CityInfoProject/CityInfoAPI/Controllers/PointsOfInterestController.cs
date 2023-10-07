using AutoMapper;
using CityInfoAPI.Data;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Models.DatabaseSessionConnection;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterests")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityRepository _cityRepository;
        private readonly CitiesDataStore _cityDataStore;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, 
            ICityRepository cityRepository,CitiesDataStore cityDataStore,
            IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService;
            _cityRepository = cityRepository;
            _cityDataStore = cityDataStore;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestsDto>>> GetPointsOfInterests(int cityId)
        {
            var pointOfInterests = await _cityRepository.GetPointsOfInterestsAsync(cityId);
            if(pointOfInterests == null)
            {
                _logger.LogCritical($"No city found with Id: {cityId}");
                return NotFound($"No city found with Id: {cityId}");
            }
            _logger.Log(LogLevel.Information,message:"All points of interests are listed.");
            return Ok(_mapper.Map<IEnumerable<PointsOfInterestsDto>>(pointOfInterests));
        }

        [HttpGet("{pointsofinterestid}")]
        public async Task<ActionResult<PointsOfInterestsDto>> GetPointOfInterestById(int cityId, int pointsofinterestid)
        {
            var pointOfInterest = await _cityRepository.GetPointOfInterestByIdAsync(cityId, pointsofinterestid);
            if (pointOfInterest == null)
            {
                return NotFound($"No point of interest found with Id: {pointsofinterestid}");
            }
            return Ok(_mapper.Map<PointsOfInterestsDto>(pointOfInterest));
        }

        [HttpPost]
        public ActionResult<PointsOfInterestsDto> CreatePointsOfInterests(int cityId,string sender,string receiver, CreationPointsOfInterestDto creationPointsOfInterestDto)
        {
            var cities = _cityDataStore.Cities;
            var city = cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound($"No city found with Id: {cityId}");
            }
            PointsOfInterestsDto newPointsOfInterest = new PointsOfInterestsDto()
            {
                Id =  cities.SelectMany(c => c.PointsOfInterests).Max(p => p.Id) + 1,
                Name = creationPointsOfInterestDto.Name,
                Description = creationPointsOfInterestDto.Description,
            };
            city.PointsOfInterests.Add(newPointsOfInterest);
            _mailService.SendMail(sender,receiver, "New Points of Interest is added");
            return Ok(newPointsOfInterest);
        }
        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, string sender, string receiver, CreationPointsOfInterestDto pointsOfInterestDto)
        {
            var cities = _cityDataStore.Cities;
            var city = cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null) return NotFound($"No city found with Id: {cityId}");
            var interestToUpdate = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId); 
            if(interestToUpdate == null) return NotFound($"No point of interest found with Id: {pointOfInterestId}");
            interestToUpdate.Name = pointsOfInterestDto.Name;
            interestToUpdate.Description = pointsOfInterestDto.Description;
            _mailService.SendMail(sender, receiver, $"Point of interest found with Id: {pointOfInterestId} is changed as {pointsOfInterestDto.ToString()}");
            return NoContent();
        }
        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, string sender, string receiver, int pointOfInterestId)
        {
            var cities = _cityDataStore.Cities;
            var city = cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound($"No city found with Id: {cityId}");
            var deletePointOfInterest = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            if(deletePointOfInterest == null) return NotFound($"No point of interest found with Id: {pointOfInterestId}");
            city.PointsOfInterests.Remove(deletePointOfInterest);
            _mailService.SendMail(sender, receiver, $"Point of interest found with Id: {pointOfInterestId} and detailes as {deletePointOfInterest.ToString()} is deleted!");
            return NoContent();
        }

    }
}

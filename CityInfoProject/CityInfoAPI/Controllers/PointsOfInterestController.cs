using AutoMapper;
using CityInfoAPI.Data;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Models.DatabaseSessionConnection;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterests")]
    [Authorize(Policy = "MustBeFromOslo")]
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
        public async Task<ActionResult<PointOfInterest>> CreatePointsOfInterests(int cityId,string sender,string receiver, CreationPointsOfInterestDto creationPointsOfInterestDto)
        {
            var city = await _cityRepository.CityExistsAsync(cityId);
            if (city == false)
            {
                return NotFound($"No city found with Id: {cityId}");
            }
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(creationPointsOfInterestDto);
            await _cityRepository.AddPointOfInterest(cityId, finalPointOfInterest);
            await _cityRepository.SaveChangesAsync();
            var createdPointOfInterest = _mapper.Map<PointsOfInterestsDto>(finalPointOfInterest);
            _mailService.SendMail(sender,receiver, "New Points of Interest is added");
            return Ok(createdPointOfInterest);
        }
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, string sender, string receiver, CreationPointsOfInterestDto pointsOfInterestDto)
        {
            if(!(await _cityRepository.CityExistsAsync(cityId)))
             return NotFound($"No city found with Id: {cityId}");
            var interestToUpdate = await _cityRepository.GetPointOfInterestByIdAsync(cityId, pointOfInterestId); 
            if(interestToUpdate == null) return NotFound($"No point of interest found with Id: {pointOfInterestId}");
            _mapper.Map(pointsOfInterestDto,interestToUpdate);
           await _cityRepository.SaveChangesAsync();
            _mailService.SendMail(sender, receiver, $"Point of interest found with Id: {pointOfInterestId} is changed as {pointsOfInterestDto.ToString()}");
            return NoContent();
        }
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, string sender, string receiver, int pointOfInterestId)
        {
            if (!(await _cityRepository.CityExistsAsync(cityId)))
                return NotFound($"No city found with Id: {cityId}");
            bool result = await _cityRepository.DeletePointOfInterestAsync(cityId, pointOfInterestId);
            if(result)
            {
                _mailService.SendMail(sender, receiver, $"Point of interest found with Id: {pointOfInterestId} is deleted!");
                return NoContent();
            }
            else return NotFound($"No interest found with Id: {pointOfInterestId}");
        }

    }
}

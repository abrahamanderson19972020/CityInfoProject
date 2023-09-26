using CityInfoAPI.Data;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterests")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PointsOfInterestsDto>> GetPointsOfInterests(int cityId)
        {
            var cities = CitiesDataStore.GetAllCities();
            var city = cities.FirstOrDefault<CityDto>(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound($"No city found with Id: {cityId}");
            }
            
            return Ok(city.PointsOfInterests);
        }

        [HttpGet("{pointsofinterestid}")]
        public ActionResult<PointsOfInterestsDto> GetPointOfInterestById(int cityId, int pointsofinterestid)
        {
            var city = CitiesDataStore.GetAllCities().FirstOrDefault(c => c.Id == cityId);  
            if(city == null)
            {
                return NotFound($"No city found with Id: {cityId}");
            }
            var pointOfInterest = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointsofinterestid);
            if(pointOfInterest == null)
            {
                return NotFound("No point of interest found!");
            }
            return Ok(pointOfInterest);
        }
      
    }
}

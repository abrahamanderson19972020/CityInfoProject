using CityInfoAPI.Entities;

namespace CityInfoAPI.Models.DatabaseSessionConnection
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityByIdAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestByIdAsync(int cityId, int pointsOfInterestId);

    }
}

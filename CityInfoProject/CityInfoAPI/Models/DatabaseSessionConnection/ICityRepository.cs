using CityInfoAPI.Data;
using CityInfoAPI.Entities;

namespace CityInfoAPI.Models.DatabaseSessionConnection
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? cityName, string? searchString, int pageNumber, int pageSize);
        Task<City?> GetCityByIdAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestByIdAsync(int cityId, int pointsOfInterestId);
        Task<bool> CityExistsAsync(int cityId);
        Task AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
        Task<bool> DeletePointOfInterestAsync(int cityId, int pointOfInterestId);
    }
}

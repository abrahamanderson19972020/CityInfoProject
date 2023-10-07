using CityInfoAPI.Entities;
using CityInfoAPI.Models.DatabaseSessionConnection;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.Services
{
    public class CityRepositoryManager : ICityRepository
    {
        private readonly CityDbContext _context;
        public CityRepositoryManager(CityDbContext context)
        {
          _context = context;
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c=>c.Name).ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int cityId)
        {
            return await _context.Cities.Include(c=> c.PointsOfInterests).FirstOrDefaultAsync(c=> c.Id == cityId);   
        }

        public async Task<PointOfInterest?> GetPointOfInterestByIdAsync(int cityId, int pointsOfInterestId)
        {
            return await _context.PointOfInterests.FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointsOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsAsync(int cityId)
        {
            return await _context.PointOfInterests.Where(p=> p.CityId == cityId).ToListAsync();
        }
    }
}

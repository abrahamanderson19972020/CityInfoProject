using CityInfoAPI.Data;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
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

        public async Task<bool> CityExistsAsync(int cityId)
        {
            var city = _context.Cities.FirstOrDefaultAsync(c=>c.Id == cityId); 
            return city != null;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c=>c.Name).ToListAsync();
        }

        public async Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? cityName, string? searchString, int pageNumber, int pageSize)
        {
             //Collestion to start from   
             var collection = _context.Cities as IQueryable<City>;
            if(!string.IsNullOrWhiteSpace(cityName))
            {
                cityName = cityName.Trim();
                collection = collection.Where(c => c.Name.ToLower() == cityName.ToLower());
            }
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                collection = collection.Where(c=> c.Name.ToLower().Contains(searchString.ToLower())
                || c.Description.ToLower().Contains(searchString.ToLower()));   

            }
            var totalItemCount = await collection.CountAsync();
            var paginationMetaData = new PaginationMetaData(totalItemCount,pageSize,pageNumber);
            //We should add paging functionality last
            var collectionToReturn =  await collection.OrderBy(c => c.Name)
                .Skip(pageSize*(pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (collectionToReturn, paginationMetaData);
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

        public async Task AddPointOfInterest(int cityId,PointOfInterest pointOfInterest)
        {
            var cityExist = await GetCityByIdAsync(cityId);
            if (cityExist != null)
            {
                cityExist.PointsOfInterests.Add(pointOfInterest);
            }
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            var city = await GetCityByIdAsync(cityId);
            if (city != null)
            {
                var pointOfInterestToChange = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
                if (pointOfInterestToChange != null)
                {
                    _context.PointOfInterests.Remove(pointOfInterestToChange);
                    await SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}

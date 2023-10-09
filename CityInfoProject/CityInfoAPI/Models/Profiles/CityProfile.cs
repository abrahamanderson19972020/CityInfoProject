using AutoMapper;
using CityInfoAPI.Entities;

namespace CityInfoAPI.Models.Profiles
{
    public class CityProfile:Profile// This comes from AutoMapper
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>();
            CreateMap<PointOfInterest, PointsOfInterestsDto>();
            CreateMap<CreationPointsOfInterestDto, PointOfInterest>();
        }
    }
}

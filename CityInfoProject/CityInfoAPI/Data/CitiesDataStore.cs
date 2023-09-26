using CityInfoAPI.Models;

namespace CityInfoAPI.Data
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {     
        }

        public static List<CityDto> GetAllCities()
        {
            return new List<CityDto>()
            {
                new CityDto() 
                { Id = 1, Name = "Kristiansand", Description = "5.biggest city",
                PointsOfInterests = new List<PointsOfInterestsDto>()
                    {
                    new PointsOfInterestsDto()
                        {
                        Id = 1,
                        Name = "Dom Kirke",
                        Description = "Central Church in Kristiansand"
                        },
                    new PointsOfInterestsDto()
                        {
                        Id = 2,
                        Name = "Centrum",
                        Description = "Central Place in Kristiansand"
                        },
                    } 
                }
                ,
                new CityDto() { Id = 2, Name = "Oslo", Description = "Capital of Norway",
                PointsOfInterests = new List<PointsOfInterestsDto>()
                {
                    new PointsOfInterestsDto()
                    {
                        Id = 1,
                        Name = "Viking Museum",
                        Description = "Oldest and fameous museum in Oslo"
                    },
                    new PointsOfInterestsDto()
                    {
                        Id = 2,
                        Name = "Istanbul Khebab",
                        Description = "Best khebab in Oslo"
                    }
                }
                },
                new CityDto(){ Id = 3, Name = "Boston", Description = "Turgut and Marinda's city",
                PointsOfInterests= new List<PointsOfInterestsDto>()
                {
                    new PointsOfInterestsDto(){
                    Id = 1,
                    Name = "Harvars University",
                    Description = "Best University in the world"
                    },
                    new PointsOfInterestsDto(){
                    Id = 2,
                    Name = "Central Park",
                    Description = "Central Park in Boston"
                    },
                }
                },
                new CityDto{ Id = 4, Name = "Bergen", Description = "The most rainy city",
                PointsOfInterests = new List<PointsOfInterestsDto> (){
                     new PointsOfInterestsDto(){ 
                     Id= 1,
                     Name="Bergen Castle",
                     Description = "Historical castle in Bergen"
                     },
                     new PointsOfInterestsDto(){
                     Id= 1,
                     Name="Bergen Castle",
                     Description = "Historical castle in Bergen"
                     },
                    }
                }
            };
        }
    }
}

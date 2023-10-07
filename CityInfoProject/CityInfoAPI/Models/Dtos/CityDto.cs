namespace CityInfoAPI.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<PointsOfInterestsDto> PointsOfInterests { get; set; } = new List<PointsOfInterestsDto>();
        public int TotalPoints { get
            {
                return PointsOfInterests.Count;
            }  
        } 
    }
}

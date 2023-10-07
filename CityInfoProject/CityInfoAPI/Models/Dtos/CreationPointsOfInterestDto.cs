using System.ComponentModel.DataAnnotations;

namespace CityInfoAPI.Models
{
    public class CreationPointsOfInterestDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}

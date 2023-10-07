using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfoAPI.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } 
        [MaxLength(500)]
        public string Description { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; } = null!;
        public int CityId { get; set; }
    }
}

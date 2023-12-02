using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The code must be minimum 3 charachters")]
        [MaxLength(3, ErrorMessage = "The code must be maximum 3 charachters")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

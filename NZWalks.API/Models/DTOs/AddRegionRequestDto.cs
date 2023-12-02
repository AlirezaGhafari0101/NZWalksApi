namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

namespace PresentationAPI.Models
{
    public class AssetModel
    {
        public int AssetId { get; set; }
        public string UserId { get; set; } = null!;
        public int? CountryId { get; set; }
        public string AssetName { get; set; } = null!;
        public decimal PriceUsd { get; set; }
        public string Type { get; set; } = null!;
        public string? Request { get; set; }
    }
}

namespace srj.Application.Dtos.Response.Silver;

public class SilverJewelryDetailsResponse
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal WeightInGrams { get; set; }

    public decimal SilverMakingChargePerGram { get; set; }

    public decimal Purity { get; set; }

    public string? BarcodeImageUrl { get; set; }
}
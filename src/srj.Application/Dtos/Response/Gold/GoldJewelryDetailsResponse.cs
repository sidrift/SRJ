using srj.Domain.Enums;

namespace srj.Application.Dtos.Response.Gold;

public class GoldJewelryDetailsResponse
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal WeightInGrams { get; set; }

    public bool HasStones { get; set; }

    public decimal? StoneWeight { get; set; }

    public string? StoneType { get; set; }

    public decimal? StonePrice { get; set; }

    public decimal PurityInPercentage { get; set; }

    public MakingChargeType MakingChargeType { get; set; }

    public decimal? MakingChargePercentage { get; set; }

    public decimal MakingChargeWeight { get; set; }

    public decimal HallMarkCharge { get; set; }

    public string? BarcodeImageUrl { get; set; }
}
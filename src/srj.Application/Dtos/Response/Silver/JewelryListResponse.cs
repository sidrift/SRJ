using srj.Domain.Enums;

namespace srj.Application.Dtos.Response.Silver;

public class JewelryListResponse
{
    public Guid Id { get; set; }

    public string Sku { get; set; } = string.Empty;

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public Metal Metal { get; set; }

    public decimal WeightInGrams { get; set; }

    public decimal Purity { get; set; }

    public bool HasStones { get; set; }

    public decimal? StoneWeight { get; set; }

    public string? StoneType { get; set; }

    public decimal? StonePrice { get; set; }

    public MakingChargeType? MakingChargeType { get; set; }

    public decimal? MakingChargePercentage { get; set; }

    public decimal? MakingChargeWeight { get; set; }

    public decimal? HallMarkCharge { get; set; }

    public DateTime CreatedAt { get; set; }
}
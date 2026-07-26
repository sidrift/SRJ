using srj.Domain.Enums;

namespace srj.Application.Dtos.Request.Jewelery;

public class JewelrySearchRequest
{
    public string? Sku { get; set; }

    public int? CategoryId { get; set; }

    public Metal? Metal { get; set; }

    public decimal? Purity { get; set; }

    public ComparisonType? PurityComparison { get; set; }

    public decimal? Weight { get; set; }

    public ComparisonType? WeightComparison { get; set; }

    public bool? HasStones { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}
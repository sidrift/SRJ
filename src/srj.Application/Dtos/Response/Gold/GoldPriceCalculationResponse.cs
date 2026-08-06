namespace srj.Application.Dtos.Response.Gold;

public class GoldPriceCalculationResponse
{
    public string Sku { get; set; } = string.Empty;

    public decimal GrossWeight { get; set; }

    public decimal StoneWeight { get; set; }

    public decimal NetGoldWeight { get; set; }

    public decimal GoldRatePerGram { get; set; }

    public decimal MakingChargeWeight { get; set; }

    public decimal? MakingChargePercentage { get; set; }

    public decimal TotalChargeableGoldWeight { get; set; }

    public decimal GoldAmount { get; set; }

    public decimal StoneAmount { get; set; }

    public decimal HallmarkAmount { get; set; }

    public decimal TotalAmountExcludingGst { get; set; }

    public decimal EstimateGstAmount { get; set; }

    public decimal TotalAmountIncludingGst { get; set; }

    public decimal PurityInPercentage { get; set; }

    public decimal TodaysGoldPricePure { get; set; }

    public string? StoneType { get; set; }

    public bool HasStones { get; set; }
}
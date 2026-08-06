namespace srj.Application.Dtos.Response.Silver;

public class SilverPriceCalculationResponse
{
    public string Sku { get; set; } = string.Empty;

    public decimal WeightInGrams { get; set; }

    public decimal SilverRatePerGram { get; set; }

    public decimal MakingChargePerGram { get; set; }

    public decimal FinalRatePerGram { get; set; }

    public decimal TotalAmountExcludingGst { get; set; }

    public decimal EstimateGstAmount { get; set; }

    public decimal TotalAmountIncludingGst { get; set; }

    public decimal PurityInPercentage { get; set; }
}
namespace srj.Application.Dtos.Response.Silver;

public class SilverPriceCalculationResponse
{
    public string Sku { get; set; } = string.Empty;

    public decimal WeightInGrams { get; set; }

    public decimal SilverRatePerGram { get; set; }

    public decimal MakingChargePerGram { get; set; }

    public decimal FinalRatePerGram { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PurityInPercentage { get; set; }
}
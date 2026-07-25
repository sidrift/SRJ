namespace srj.Application.Dtos.Response.Gold;

public class GoldPriceCalculationResponse
{
    public string Sku { get; set; } = string.Empty;

    public decimal GrossWeight { get; set; }

    public decimal StoneWeight { get; set; }

    public decimal NetGoldWeight { get; set; }

    public decimal GoldRatePerGram { get; set; }

    public decimal MakingChargeWeight { get; set; }

    public decimal TotalChargeableGoldWeight { get; set; }

    public decimal GoldAmount { get; set; }

    public decimal StoneAmount { get; set; }

    public decimal HallmarkAmount { get; set; }

    public decimal TotalAmount { get; set; }
}
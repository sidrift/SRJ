namespace srj.Application.Dtos.Response.Silver;

public class SilverPriceResponse
{
    public DateOnly PriceDate { get; set; }

    public decimal SellPriceSilly { get; set; }

    public decimal BuyPriceSilly { get; set; }

    public decimal SellPrice99 { get; set; }

    public decimal BuyPrice99 { get; set; }

    public DateTime UpdatedAt { get; set; }
}
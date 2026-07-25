namespace srj.Domain.Models;

public class GoldPrice
{
    public long Id { get; set; }

    public DateOnly PriceDate { get; set; }

    public decimal SellPrice24KImp { get; set; }

    public decimal BuyPrice24KImp { get; set; }

    public decimal SellPrice24KFt { get; set; }

    public decimal BuyPrice24KFt { get; set; }

    // Calculated automatically
    public decimal SellPrice22K { get; set; }

    public decimal SellPrice20K { get; set; }

    public decimal SellPrice18K { get; set; }

    public DateTime UpdatedAt { get; set; }
}
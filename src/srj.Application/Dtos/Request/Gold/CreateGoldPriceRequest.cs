namespace srj.Application.Dtos.Request.Gold;

public class CreateGoldPriceRequest
{
    public decimal SellPrice24KImp { get; set; }

    public decimal BuyPrice24KImp { get; set; }

    public decimal SellPrice24KFt { get; set; }

    public decimal BuyPrice24KFt { get; set; }
}
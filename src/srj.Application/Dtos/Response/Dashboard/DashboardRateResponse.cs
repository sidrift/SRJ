namespace srj.Application.Dtos.Response.Dashboard;

public class DashboardRateResponse
{
    public DateOnly PriceDate { get; set; }

    public GoldDashboardRate? Gold { get; set; }

    public SilverDashboardRate? Silver { get; set; }
}

public class GoldDashboardRate
{
    public decimal SellPrice24KImp { get; set; }

    public decimal BuyPrice24KImp { get; set; }

    public decimal SellPrice24KFt { get; set; }

    public decimal BuyPrice24KFt { get; set; }

    public decimal SellPrice22K { get; set; }

    public decimal SellPrice20K { get; set; }

    public decimal SellPrice18K { get; set; }
}

public class SilverDashboardRate
{
    public decimal SellPriceSilly { get; set; }

    public decimal BuyPriceSilly { get; set; }

    public decimal SellPrice99 { get; set; }

    public decimal BuyPrice99 { get; set; }
}
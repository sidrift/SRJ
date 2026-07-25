using srj.Application.Dtos.Response.Dashboard;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Dashboard;

namespace srj.Application.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IIndiaDateTimeService _dateTimeService;
    private readonly IGoldRateRepository _goldRateRepository;
    private readonly ISilverRateRepository _silverRateRepository;

    public DashboardService(
        IGoldRateRepository goldRateRepository,
        ISilverRateRepository silverRateRepository,
        IIndiaDateTimeService dateTimeService)
    {
        _goldRateRepository = goldRateRepository;
        _silverRateRepository = silverRateRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<DashboardRateResponse?> GetTodayRatesAsync()
    {
        var today = _dateTimeService.Today;


        var goldRate =
            await _goldRateRepository.GetByDateAsync(today);


        var silverRate =
            await _silverRateRepository.GetByDateAsync(today);


        if (goldRate == null && silverRate == null)
            return null;


        return new DashboardRateResponse
        {
            PriceDate = today,

            Gold = goldRate == null
                ? null
                : new GoldDashboardRate
                {
                    SellPrice24KImp = goldRate.SellPrice24KImp,
                    BuyPrice24KImp = goldRate.BuyPrice24KImp,

                    SellPrice24KFt = goldRate.SellPrice24KFt,
                    BuyPrice24KFt = goldRate.BuyPrice24KFt,

                    SellPrice22K = goldRate.SellPrice22K,
                    SellPrice20K = goldRate.SellPrice20K,
                    SellPrice18K = goldRate.SellPrice18K
                },


            Silver = silverRate == null
                ? null
                : new SilverDashboardRate
                {
                    SellPriceSilly = silverRate.SellPriceSilly,
                    BuyPriceSilly = silverRate.BuyPriceSilly,

                    SellPrice99 = silverRate.SellPrice99,
                    BuyPrice99 = silverRate.BuyPrice99
                }
        };
    }
}
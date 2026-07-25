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

    public async Task<DashboardRateResponse?> GetRatesAsync(DateOnly date)
    {
        var goldRate =
            await _goldRateRepository.GetByDateAsync(date);

        var silverRate =
            await _silverRateRepository.GetByDateAsync(date);

        return new DashboardRateResponse
        {
            PriceDate = date,

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
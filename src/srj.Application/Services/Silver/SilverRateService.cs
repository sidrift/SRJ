using srj.Application.Dtos.Request.Silver;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Silver;
using srj.Domain.Models;

namespace srj.Application.Services.Silver;

public class SilverRateService : ISilverRateService
{
    private readonly IIndiaDateTimeService _indiaDateTime;
    private readonly ISilverRateRepository _repo;

    public SilverRateService(ISilverRateRepository repo,
        IIndiaDateTimeService indiaDateTime)
    {
        _repo = repo;
        _indiaDateTime = indiaDateTime;
    }

    public async Task<SilverPrice> UpsertAsync(CreateSilverPriceRequest request)
    {
        var price = new SilverPrice
        {
            PriceDate = _indiaDateTime.Today,

            SellPriceSilly = request.SellPriceSilly,
            BuyPriceSilly = request.BuyPriceSilly,

            SellPrice99 = request.SellPrice99,
            BuyPrice99 = request.BuyPrice99,

            UpdatedAt = DateTime.UtcNow
        };

        return await _repo.UpsertAsync(price);
    }

    public Task<SilverPrice?> GetTodayAsync()
    {
        return _repo.GetTodayAsync();
    }

    public Task<SilverPrice?> GetByDateAsync(DateOnly date)
    {
        return _repo.GetByDateAsync(date);
    }

    public Task<List<SilverPrice>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }
}
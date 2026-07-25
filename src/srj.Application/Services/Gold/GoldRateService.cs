using srj.Application.Dtos.Request.Gold;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Gold;
using srj.Domain.Models;

namespace srj.Application.Services.Gold;

public class GoldRateService : IGoldRateService
{
    private readonly IIndiaDateTimeService _indiaDateTime;
    private readonly IGoldRateRepository _repo;

    public GoldRateService(IGoldRateRepository repo,
        IIndiaDateTimeService indiaDateTime)
    {
        _repo = repo;
        _indiaDateTime = indiaDateTime;
    }

    public async Task<GoldPrice> UpsertAsync(CreateGoldPriceRequest request)
    {
        var goldPrice = new GoldPrice
        {
            PriceDate = _indiaDateTime.Today,

            SellPrice24KImp = request.SellPrice24KImp,
            BuyPrice24KImp = request.BuyPrice24KImp,

            SellPrice24KFt = request.SellPrice24KFt,
            BuyPrice24KFt = request.BuyPrice24KFt,

            // Auto-calculated
            SellPrice22K = Math.Round(request.SellPrice24KImp * 0.916m, 2),
            SellPrice20K = Math.Round(request.SellPrice24KImp * 0.833m, 2),
            SellPrice18K = Math.Round(request.SellPrice24KImp * 0.750m, 2),

            UpdatedAt = DateTime.UtcNow
        };

        return await _repo.UpsertAsync(goldPrice);
    }

    public Task<GoldPrice?> GetTodayAsync()
    {
        return _repo.GetTodayAsync();
    }

    public Task<GoldPrice?> GetByDateAsync(DateOnly date)
    {
        return _repo.GetByDateAsync(date);
    }

    public Task<List<GoldPrice>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }
}
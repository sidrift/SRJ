using srj.Application.Dtos.Request.Gold;
using srj.Domain.Models;

namespace srj.Application.Interface.Services.Gold;

public interface IGoldRateService
{
    Task<GoldPrice> UpsertAsync(CreateGoldPriceRequest request);

    Task<GoldPrice?> GetTodayAsync();

    Task<GoldPrice?> GetByDateAsync(DateOnly date);

    Task<List<GoldPrice>> GetAllAsync();
}
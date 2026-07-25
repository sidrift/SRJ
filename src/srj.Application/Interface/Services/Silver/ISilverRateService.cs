using srj.Application.Dtos.Request.Silver;
using srj.Domain.Models;

namespace srj.Application.Interface.Services.Silver;

public interface ISilverRateService
{
    Task<SilverPrice> UpsertAsync(CreateSilverPriceRequest request);

    Task<SilverPrice?> GetTodayAsync();

    Task<SilverPrice?> GetByDateAsync(DateOnly date);

    Task<List<SilverPrice>> GetAllAsync();
}
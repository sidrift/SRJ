using srj.Domain.Models;

namespace srj.Application.Interface.Repository;

public interface IGoldRateRepository
{
    Task<GoldPrice?> GetTodayAsync();

    Task<GoldPrice?> GetByDateAsync(DateOnly date);

    Task<List<GoldPrice>> GetAllAsync();

    Task<GoldPrice> UpsertAsync(GoldPrice price);
}
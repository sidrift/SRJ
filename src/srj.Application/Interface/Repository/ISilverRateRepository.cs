using srj.Domain.Models;

namespace srj.Application.Interface.Repository;

public interface ISilverRateRepository
{
    Task<SilverPrice?> GetTodayAsync();

    Task<SilverPrice?> GetByDateAsync(DateOnly date);

    Task<List<SilverPrice>> GetAllAsync();

    Task<SilverPrice> UpsertAsync(SilverPrice price);
}
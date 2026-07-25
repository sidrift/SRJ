using Microsoft.EntityFrameworkCore;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Domain.Models;
using srj.Infrastructure.DbContext;

namespace srj.Infrastructure.Repositories;

public class SilverRateRepository : ISilverRateRepository
{
    private readonly JewelryDbContext _context;
    private readonly IIndiaDateTimeService _dateTimeService;

    public SilverRateRepository(JewelryDbContext context,
        IIndiaDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<SilverPrice?> GetTodayAsync()
    {
        var today = _dateTimeService.Today;

        return await _context.SilverPrices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PriceDate == today);
    }

    public async Task<SilverPrice?> GetByDateAsync(DateOnly date)
    {
        return await _context.SilverPrices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PriceDate == date);
    }

    public async Task<List<SilverPrice>> GetAllAsync()
    {
        return await _context.SilverPrices
            .AsNoTracking()
            .OrderByDescending(x => x.PriceDate)
            .ToListAsync();
    }

    public async Task<SilverPrice> UpsertAsync(SilverPrice price)
    {
        var today = _dateTimeService.Today;

        if (price.PriceDate != today)
            throw new InvalidOperationException(
                "Silver price can only be added or updated for today's date.");

        var existing = await _context.SilverPrices
            .FirstOrDefaultAsync(x => x.PriceDate == today);

        if (existing == null)
        {
            price.UpdatedAt = DateTime.UtcNow;

            _context.SilverPrices.Add(price);

            await _context.SaveChangesAsync();

            return price;
        }

        existing.SellPriceSilly = price.SellPriceSilly;
        existing.BuyPriceSilly = price.BuyPriceSilly;

        existing.SellPrice99 = price.SellPrice99;
        existing.BuyPrice99 = price.BuyPrice99;

        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existing;
    }
}
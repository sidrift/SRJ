using Microsoft.EntityFrameworkCore;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Domain.Models;
using srj.Infrastructure.DbContext;

namespace srj.Infrastructure.Repositories;

public class GoldRateRepository : IGoldRateRepository
{
    private readonly JewelryDbContext _context;
    private readonly IIndiaDateTimeService _dateTimeService;

    public GoldRateRepository(JewelryDbContext context,
        IIndiaDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<GoldPrice?> GetTodayAsync()
    {
        var today = _dateTimeService.Today;

        return await _context.GoldPrices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PriceDate == today);
    }

    public async Task<GoldPrice?> GetByDateAsync(DateOnly date)
    {
        return await _context.GoldPrices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PriceDate == date);
    }

    public async Task<List<GoldPrice>> GetAllAsync()
    {
        return await _context.GoldPrices
            .AsNoTracking()
            .OrderByDescending(x => x.PriceDate)
            .ToListAsync();
    }

    public async Task<GoldPrice> UpsertAsync(GoldPrice price)
    {
        var today = _dateTimeService.Today;

        if (price.PriceDate != today)
            throw new InvalidOperationException(
                "Gold price can only be added or updated for today's date.");

        var existing = await _context.GoldPrices
            .FirstOrDefaultAsync(x => x.PriceDate == today);

        if (existing == null)
        {
            price.UpdatedAt = DateTime.UtcNow;

            _context.GoldPrices.Add(price);

            await _context.SaveChangesAsync();

            return price;
        }

        existing.SellPrice24KImp = price.SellPrice24KImp;
        existing.BuyPrice24KImp = price.BuyPrice24KImp;

        existing.SellPrice24KFt = price.SellPrice24KFt;
        existing.BuyPrice24KFt = price.BuyPrice24KFt;

        existing.SellPrice22K = price.SellPrice22K;
        existing.SellPrice20K = price.SellPrice20K;
        existing.SellPrice18K = price.SellPrice18K;

        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existing;
    }
}
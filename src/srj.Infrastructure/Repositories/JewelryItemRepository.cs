using Microsoft.EntityFrameworkCore;
using srj.Application.Dtos.Commons;
using srj.Application.Dtos.Request.Jewelery;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Repository;
using srj.Domain.Enums;
using srj.Domain.Models;
using srj.Infrastructure.DbContext;

namespace srj.Infrastructure.Repositories;

public class JewelryItemRepository : IJewelryItemRepository
{
    private readonly JewelryDbContext _context;

    public JewelryItemRepository(JewelryDbContext context)
    {
        _context = context;
    }

    public async Task<JewelryItem?> GetByIdAsync(Guid id)
    {
        return await _context.JewelryItems.FindAsync(id);
    }

    public async Task<JewelryItem?> GetBySkuAsync(string sku)
    {
        return await _context.JewelryItems
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Sku == sku);
    }

    public async Task<List<JewelryItem>> GetAllAsync(Metal? metal = null, string? category = null)
    {
        var query = _context.JewelryItems.AsNoTracking().AsQueryable();

        if (metal.HasValue) query = query.Where(x => x.Category.Metal == metal.Value);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(x =>
                x.Category.Name.ToLower().Contains(category.ToLower()));

        return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<JewelryItem> CreateAsync(JewelryItem item)
    {
        item.Id = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;

        _context.JewelryItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(JewelryItem item)
    {
        _context.JewelryItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBySkuAsync(string sku)
    {
        var item = await _context.JewelryItems
            .FirstOrDefaultAsync(x => x.Sku == sku);

        if (item == null) throw new KeyNotFoundException($"Jewelry item with SKU '{sku}' was not found.");

        _context.JewelryItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasItemsByCategoryIdAsync(long categoryId)
    {
        return await _context.JewelryItems
            .AnyAsync(x => x.CategoryId == categoryId);
    }

   public async Task<PagedResponse<JewelryListResponse>> SearchAsync(JewelrySearchRequest request)
{
    var query = _context.JewelryItems
        .AsNoTracking()
        .Include(x => x.Category)
        .AsQueryable();

    // SKU
    if (!string.IsNullOrWhiteSpace(request.Sku))
    {
        query = query.Where(x =>
            x.Sku.ToLower().Contains(request.Sku.ToLower()));
    }

    // Category
    if (request.CategoryId.HasValue)
    {
        query = query.Where(x =>
            x.CategoryId == request.CategoryId.Value);
    }

    // Metal
    if (request.Metal.HasValue)
    {
        query = query.Where(x =>
            x.Category.Metal == request.Metal.Value);
    }

    // Has Stones
    if (request.HasStones.HasValue)
    {
        query = query.Where(x =>
            x.HasStones == request.HasStones.Value);
    }

    // Purity
    if (request.Purity.HasValue)
    {
        switch (request.PurityComparison)
        {
            case ComparisonType.GreaterThan:
                query = query.Where(x =>
                    x.PurityInPercentage > request.Purity.Value);
                break;

            case ComparisonType.LessThan:
                query = query.Where(x =>
                    x.PurityInPercentage < request.Purity.Value);
                break;

            default:
                query = query.Where(x =>
                    x.PurityInPercentage == request.Purity.Value);
                break;
        }
    }

    // Weight
    if (request.Weight.HasValue)
    {
        switch (request.WeightComparison)
        {
            case ComparisonType.GreaterThan:
                query = query.Where(x =>
                    x.WeightInGrams > request.Weight.Value);
                break;

            case ComparisonType.LessThan:
                query = query.Where(x =>
                    x.WeightInGrams < request.Weight.Value);
                break;

            default:
                query = query.Where(x =>
                    x.WeightInGrams == request.Weight.Value);
                break;
        }
    }

    var totalRecords = await query.CountAsync();

    var items = await query
        .OrderByDescending(x => x.CreatedAt)
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(x => new JewelryListResponse
        {
            Id = x.Id,
            Sku = x.Sku,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            Metal = x.Category.Metal,
            WeightInGrams = x.WeightInGrams,
            Purity = x.PurityInPercentage,
            HasStones = x.HasStones,
            StoneWeight = x.StoneWeight,
            StonePrice = x.StonePrice,
            HallMarkCharge = x.HallMarkCharge,
            MakingChargeType = x.MakingChargeType,
            MakingChargePercentage = x.MakingChargePercentage,
            MakingChargeWeight = x.MakingChargeWeight,
            CreatedAt = x.CreatedAt
        })
        .ToListAsync();

    return new PagedResponse<JewelryListResponse>
    {
        Items = items,
        PageNumber = request.PageNumber,
        PageSize = request.PageSize,
        TotalCount = totalRecords
    };
}
}
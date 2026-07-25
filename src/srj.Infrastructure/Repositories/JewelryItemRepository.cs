using Microsoft.EntityFrameworkCore;
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
}
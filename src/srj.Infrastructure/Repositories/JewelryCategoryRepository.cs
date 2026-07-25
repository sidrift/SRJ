using Microsoft.EntityFrameworkCore;
using srj.Application.Interface.Repository;
using srj.Domain.Enums;
using srj.Domain.Models;
using srj.Infrastructure.DbContext;

namespace srj.Infrastructure.Repositories;

public class JewelryCategoryRepository : IJewelryCategoryRepository
{
    private readonly JewelryDbContext _repo;

    public JewelryCategoryRepository(JewelryDbContext repo)
    {
        _repo = repo;
    }

    public async Task<ItemCategory?> GetByIdAsync(long id)
    {
        return await _repo.ItemCategories.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<ItemCategory>> GetAllAsync(Metal? metal = null, string? name  = null)
    {
        var query = _repo.ItemCategories.AsQueryable();

        if (metal.HasValue)
        {
            query = query.Where(x => x.Metal == metal.Value);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.Trim();

            query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        return await query
            .OrderBy(x => x.Metal)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<ItemCategory> CreateAsync(ItemCategory item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ArgumentException("Item category name cannot be empty.", nameof(item.Name));

        var exists = await _repo.ItemCategories
            .AnyAsync(x =>
                x.Name.ToLower() == item.Name.ToLower() &&
                x.Metal == item.Metal);

        if (exists)
            throw new InvalidOperationException(
                "Category already exists for this metal.");

        _repo.ItemCategories.Add(item);

        await _repo.SaveChangesAsync();

        return item;
    }

    public async Task UpdateAsync(ItemCategory item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ArgumentException("Item category name cannot be empty.", nameof(item.Name));

        var existingItem = await _repo.ItemCategories.FindAsync(item.Id);

        if (existingItem == null) throw new KeyNotFoundException($"Item category with ID {item.Id} was not found.");

        existingItem.Name = item.Name;

        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var item = await _repo.ItemCategories.FindAsync(id);

        if (item == null) throw new KeyNotFoundException($"Item category with ID {id} was not found.");

        _repo.ItemCategories.Remove(item);
        await _repo.SaveChangesAsync();
    }
}
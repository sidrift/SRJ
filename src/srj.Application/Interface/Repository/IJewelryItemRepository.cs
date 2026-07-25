using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Interface.Repository;

public interface IJewelryItemRepository
{
    Task<JewelryItem?> GetByIdAsync(Guid id);
    Task<JewelryItem?> GetBySkuAsync(string sku);
    Task<List<JewelryItem>> GetAllAsync(Metal? metal = null, string? category = null);
    Task<JewelryItem> CreateAsync(JewelryItem item);
    Task UpdateAsync(JewelryItem item);
    Task DeleteBySkuAsync(string sku);
    Task<bool> HasItemsByCategoryIdAsync(long categoryId);
}
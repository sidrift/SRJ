using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Interface.Repository;

public interface IItemCategoryRepository
{
    Task<ItemCategory?> GetByIdAsync(long id);
    Task<List<ItemCategory>> GetAllAsync(Metal? metal = null, string? name = null);
    Task<ItemCategory> CreateAsync(ItemCategory item);
    Task UpdateAsync(ItemCategory item);
    Task DeleteAsync(long id);
}
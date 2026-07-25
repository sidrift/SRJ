using srj.Application.Interface.Repository;
using srj.Application.Interface.Services.Items;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services.Items;

public class JewelryCategoryService : IJewelryCategoryService
{
    private readonly IJewelryCategoryRepository _categoryRepository;
    private readonly IJewelryItemRepository _itemRepository;

    public JewelryCategoryService(IJewelryCategoryRepository categoryRepository,
        IJewelryItemRepository itemRepository)
    {
        _categoryRepository = categoryRepository;
        _itemRepository = itemRepository;
    }

    public async Task<ItemCategory?> GetByIdAsync(long id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<List<ItemCategory>> GetAllAsync(Metal? metal = null)
    {
        return await _categoryRepository.GetAllAsync(metal);
    }

    public async Task<ItemCategory> CreateAsync(ItemCategory item)
    {
        if (string.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException("Category name is required.");

        return await _categoryRepository.CreateAsync(item);
    }

    public async Task UpdateAsync(ItemCategory item)
    {
        if (string.IsNullOrWhiteSpace(item.Name)) throw new ArgumentException("Category name is required.");

        await _categoryRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(long id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        var hasJewelryItems = await _itemRepository.HasItemsByCategoryIdAsync(id);

        if (hasJewelryItems)
            throw new InvalidOperationException(
                "Cannot delete category because jewellery items exist under this category.");

        await _categoryRepository.DeleteAsync(id);
    }
}
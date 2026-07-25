using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Domain.Models;

namespace srj.Application.Services;

public abstract class JewelryServiceBase
{
    protected readonly IBarcodeService BarcodeService;
    protected readonly IJewelryCategoryRepository CategoryRepository;
    protected readonly IFileStorageService FileStorage;
    protected readonly IJewelryItemRepository Repository;
    protected readonly ISkuGenerator SkuGenerator;

    protected JewelryServiceBase(
        IJewelryItemRepository repository,
        IJewelryCategoryRepository categoryRepository,
        ISkuGenerator skuGenerator,
        IBarcodeService barcodeService,
        IFileStorageService fileStorage)
    {
        Repository = repository;
        CategoryRepository = categoryRepository;
        SkuGenerator = skuGenerator;
        BarcodeService = barcodeService;
        FileStorage = fileStorage;
    }

    protected async Task<ItemCategory> GetCategoryAsync(long categoryId)
    {
        var category = await CategoryRepository.GetByIdAsync(categoryId);

        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        return category;
    }

    protected async Task<(ItemCategory Category, string Sku, string BarcodeUrl)>
        PrepareNewItemAsync(long categoryId, decimal weight)
    {
        var category = await GetCategoryAsync(categoryId);

        var sku = await SkuGenerator.GenerateAsync(category.Metal, category);

        var barcode = await BarcodeService.GenerateBarcodeAsync(sku, weight);

        return (category, sku, barcode);
    }

    protected async Task RegenerateBarcodeAsync(JewelryItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.BarcodeImageUrl)) await FileStorage.DeleteFileAsync(item.BarcodeImageUrl);

        item.BarcodeImageUrl =
            await BarcodeService.GenerateBarcodeAsync(
                item.Sku,
                item.WeightInGrams);
    }

    public virtual async Task<JewelryItem> GetBySkuAsync(string sku)
    {
        var item = await Repository.GetBySkuAsync(sku);

        if (item == null)
            throw new KeyNotFoundException(
                $"Jewelry item '{sku}' was not found.");

        return item;
    }

    public virtual async Task DeleteBySkuAsync(string sku)
    {
        var item = await GetBySkuAsync(sku);

        if (!string.IsNullOrWhiteSpace(item.BarcodeImageUrl)) await FileStorage.DeleteFileAsync(item.BarcodeImageUrl);

        await Repository.DeleteBySkuAsync(sku);
    }
}
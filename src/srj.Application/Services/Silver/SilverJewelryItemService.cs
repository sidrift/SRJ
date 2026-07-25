using srj.Application.Dtos.Request.Silver;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Silver;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services.Silver;

public class SilverJewelryItemService : JewelryServiceBase, ISilverJewelryItemService
{
    public SilverJewelryItemService(
        IJewelryItemRepository repository,
        IJewelryCategoryRepository categoryRepository,
        ISkuGenerator skuGenerator,
        IBarcodeService barcodeService,
        IFileStorageService fileStorage)
        : base(
            repository,
            categoryRepository,
            skuGenerator,
            barcodeService,
            fileStorage)
    {
    }

    public async Task<JewelryItem> CreateAsync(
        CreateSilverJewelryRequest request)
    {
        var (category, sku, barcodeUrl) =
            await PrepareNewItemAsync(
                request.CategoryId,
                request.WeightInGrams);

        if (category.Metal != Metal.Silver)
            throw new InvalidOperationException(
                "Selected category is not a Silver category.");

        var item = new JewelryItem
        {
            CategoryId = category.Id,

            Sku = sku,

            WeightInGrams = request.WeightInGrams,

            PurityInPercentage = request.Purity,

            SilverMakingChargePerGram =
                request.SilverMakingChargePerGram,

            HasStones = false,
            StoneWeight = null,
            StoneType = null,
            StonePrice = null,

            BarcodeImageUrl = barcodeUrl,

            CreatedAt = DateTime.UtcNow
        };

        return await Repository.CreateAsync(item);
    }

    public async Task<JewelryItem> UpdateAsync(UpdateSilverJewelryRequest request)
    {
        var item = await GetBySkuAsync(request.Sku);

        var category = await GetCategoryAsync(request.CategoryId);

        if (category.Metal != Metal.Silver)
            throw new InvalidOperationException(
                "The selected category does not belong to Silver.");

        var weightChanged = item.WeightInGrams != request.WeightInGrams;

        item.CategoryId = category.Id;

        item.WeightInGrams = request.WeightInGrams;

        item.PurityInPercentage = request.Purity;

        item.SilverMakingChargePerGram =
            request.SilverMakingChargePerGram;

        item.HasStones = false;
        item.StoneWeight = null;
        item.StoneType = null;
        item.StonePrice = null;

        if (weightChanged) await RegenerateBarcodeAsync(item);

        await Repository.UpdateAsync(item);

        return item;
    }
}
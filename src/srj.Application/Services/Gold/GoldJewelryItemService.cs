using srj.Application.Dtos.Request.Gold;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Gold;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services.Gold;

public class GoldJewelryItemService : JewelryServiceBase, IGoldJewelryItemService
{
    public GoldJewelryItemService(
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

    public async Task<JewelryItem> CreateAsync(CreateGoldJewelryRequest request)
    {
        if (!request.HasStones)
        {
            request.StoneWeight = null;
            request.StoneType = null;
            request.StonePrice = null;
        }

        var (category, sku, barcodeUrl) =
            await PrepareNewItemAsync(
                request.CategoryId,
                request.WeightInGrams);

        if (category.Metal != Metal.Gold)
            throw new InvalidOperationException(
                "Selected category is not a Gold category.");

        var makingChargeWeight = CalculateMakingChargeWeight(
            request.WeightInGrams,
            request.MakingChargeType,
            request.MakingChargePercentage,
            request.MakingChargeWeight);

        var item = new JewelryItem
        {
            CategoryId = category.Id,

            Sku = sku,

            WeightInGrams = request.WeightInGrams,

            HasStones = request.HasStones,
            StoneWeight = request.StoneWeight,
            StoneType = request.StoneType,
            StonePrice = request.StonePrice,

            PurityInPercentage = request.Purity,

            MakingChargeType = request.MakingChargeType,

            MakingChargePercentage =
                request.MakingChargeType == MakingChargeType.Percentage
                    ? request.MakingChargePercentage
                    : null,

            MakingChargeWeight = makingChargeWeight,

            HallMarkCharge = request.HallMarkCharge,

            BarcodeImageUrl = barcodeUrl,

            CreatedAt = DateTime.UtcNow
        };

        return await Repository.CreateAsync(item);
    }

    public async Task<JewelryItem> UpdateAsync(UpdateGoldJewelryRequest request)
    {
        var item = await GetBySkuAsync(request.Sku);

        var category = await GetCategoryAsync(request.CategoryId);

        if (category.Metal != Metal.Gold)
            throw new InvalidOperationException(
                "The selected category does not belong to Gold.");

        if (!request.HasStones)
        {
            request.StoneWeight = null;
            request.StoneType = null;
            request.StonePrice = null;
        }

        var weightChanged = item.WeightInGrams != request.WeightInGrams;

        var makingChargeWeight = CalculateMakingChargeWeight(
            request.WeightInGrams,
            request.MakingChargeType,
            request.MakingChargePercentage,
            request.MakingChargeWeight);

        item.CategoryId = category.Id;

        item.WeightInGrams = request.WeightInGrams;

        item.HasStones = request.HasStones;
        item.StoneWeight = request.StoneWeight;
        item.StoneType = request.StoneType;
        item.StonePrice = request.StonePrice;

        item.PurityInPercentage = request.Purity;

        item.MakingChargeType = request.MakingChargeType;

        item.MakingChargePercentage =
            request.MakingChargeType == MakingChargeType.Percentage
                ? request.MakingChargePercentage
                : null;

        item.MakingChargeWeight = makingChargeWeight;

        item.HallMarkCharge = request.HallMarkCharge;

        if (weightChanged) await RegenerateBarcodeAsync(item);

        await Repository.UpdateAsync(item);

        return item;
    }

    private static decimal CalculateMakingChargeWeight(
        decimal weightInGrams,
        MakingChargeType makingChargeType,
        decimal? makingChargePercentage,
        decimal? makingChargeWeight)
    {
        return makingChargeType switch
        {
            MakingChargeType.Percentage when makingChargePercentage.HasValue =>
                weightInGrams * (makingChargePercentage.Value / 100m),

            MakingChargeType.Weight when makingChargeWeight.HasValue =>
                makingChargeWeight.Value,

            MakingChargeType.Percentage =>
                throw new ArgumentException(
                    "Making charge percentage is required."),

            MakingChargeType.Weight =>
                throw new ArgumentException(
                    "Making charge weight is required."),

            _ =>
                throw new InvalidOperationException(
                    "Invalid making charge type.")
        };
    }
}
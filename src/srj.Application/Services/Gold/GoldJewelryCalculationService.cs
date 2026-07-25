using srj.Application.Dtos.Response.Gold;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Gold;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services.Gold;

public class GoldJewelryCalculationService : IGoldJewelryCalculationService
{
    private readonly IIndiaDateTimeService _dateTimeService;
    private readonly IGoldRateRepository _goldRateRepository;
    private readonly IJewelryItemRepository _jewelryRepository;

    public GoldJewelryCalculationService(
        IJewelryItemRepository jewelryRepository,
        IGoldRateRepository goldRateRepository,
        IIndiaDateTimeService dateTimeService)
    {
        _jewelryRepository = jewelryRepository;
        _goldRateRepository = goldRateRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<GoldPriceCalculationResponse> CalculateAsync(string sku)
    {
        var item = await _jewelryRepository.GetBySkuAsync(sku);

        if (item == null)
            throw new KeyNotFoundException(
                $"Jewellery item '{sku}' not found.");

        if (item.Category.Metal != Metal.Gold)
            throw new InvalidOperationException(
                "This SKU is not a gold jewellery item.");

        var goldPrice = await _goldRateRepository.GetByDateAsync(_dateTimeService.Today);

        if (goldPrice == null)
            throw new InvalidOperationException(
                "Today's gold price is not available.");

        // 24K rate converted to item's purity
        var goldRatePerGram = Math.Round(
            goldPrice.SellPrice24KImp *
            (item.PurityInPercentage / 100m),
            2);

        // Remove stones from gold weight
        var netGoldWeight = item.WeightInGrams;

        if (item.HasStones) netGoldWeight -= item.StoneWeight ?? 0m;

        // Calculate making charge equivalent weight
        var makingChargeWeight = CalculateMakingChargeWeight(
            item,
            item.WeightInGrams);

        var goldWeightIncludingMaking =
            netGoldWeight + makingChargeWeight;

        var goldAmount = Math.Round(
            goldWeightIncludingMaking * goldRatePerGram,
            2);

        var stoneAmount = item.StonePrice ?? 0m;

        var hallmarkAmount = item.HallMarkCharge ?? 0m;

        var totalAmount =
            goldAmount +
            stoneAmount +
            hallmarkAmount;

        return new GoldPriceCalculationResponse
        {
            Sku = item.Sku,

            GrossWeight = item.WeightInGrams,

            StoneWeight = item.StoneWeight ?? 0m,

            NetGoldWeight = netGoldWeight,

            MakingChargeWeight = makingChargeWeight,

            TotalChargeableGoldWeight = netGoldWeight + makingChargeWeight,

            GoldRatePerGram = goldRatePerGram,

            GoldAmount = goldAmount,

            StoneAmount = stoneAmount,

            HallmarkAmount = hallmarkAmount,

            TotalAmount = totalAmount
        };
    }

    private static decimal CalculateMakingChargeWeight(
        JewelryItem item,
        decimal netWeight)
    {
        if (item.MakingChargeType == MakingChargeType.Percentage)
        {
            if (!item.MakingChargePercentage.HasValue)
                throw new InvalidOperationException(
                    "Making charge percentage missing.");

            return Math.Round(
                netWeight *
                (item.MakingChargePercentage.Value / 100m),
                3);
        }

        if (item.MakingChargeType == MakingChargeType.Weight)
        {
            if (!item.MakingChargeWeight.HasValue)
                throw new InvalidOperationException(
                    "Making charge weight missing.");

            return item.MakingChargeWeight.Value;
        }

        return 0m;
    }
}
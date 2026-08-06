using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Silver;
using srj.Domain.Enums;

namespace srj.Application.Services.Silver;

public class SilverJewelryCalculationService : ISilverJewelryCalculationService
{
    private readonly IIndiaDateTimeService _dateTimeService;
    private readonly IJewelryItemRepository _jewelryRepository;
    private readonly ISilverRateRepository _silverRateRepository;

    public SilverJewelryCalculationService(
        IJewelryItemRepository jewelryRepository,
        ISilverRateRepository silverRateRepository,
        IIndiaDateTimeService dateTimeService
    )
    {
        _jewelryRepository = jewelryRepository;
        _silverRateRepository = silverRateRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<SilverPriceCalculationResponse> CalculateAsync(string sku)
    {
        var item = await _jewelryRepository.GetBySkuAsync(sku);

        if (item == null)
            throw new KeyNotFoundException($"Jewellery item '{sku}' not found.");

        if (item.Category.Metal != Metal.Silver)
            throw new InvalidOperationException("This SKU is not a silver jewellery item.");

        var silverPrice = await _silverRateRepository.GetByDateAsync(_dateTimeService.Today);

        if (silverPrice == null)
            throw new InvalidOperationException("Today's silver price is not available.");

        // Base silver rate
        var silverRatePerGram = silverPrice.SellPriceSilly;

        // Flat making charge added per gram
        var makingChargePerGram = item.SilverMakingChargePerGram ?? 0m;

        // Final rate including making charge
        var finalRatePerGram = silverRatePerGram + makingChargePerGram;

        var totalAmountExcludingGst = Math.Round(item.WeightInGrams * finalRatePerGram, 2);

        var gstAmount = Math.Round(totalAmountExcludingGst * 0.03m, 2);

        var totalAmountIncludingGst = gstAmount + totalAmountExcludingGst;

        return new SilverPriceCalculationResponse
        {
            Sku = item.Sku,

            WeightInGrams = item.WeightInGrams,

            SilverRatePerGram = silverRatePerGram,

            MakingChargePerGram = makingChargePerGram,

            FinalRatePerGram = finalRatePerGram,

            TotalAmountExcludingGst = totalAmountExcludingGst,

            EstimateGstAmount = gstAmount,

            TotalAmountIncludingGst = totalAmountIncludingGst,

            PurityInPercentage = item.PurityInPercentage,
        };
    }
}
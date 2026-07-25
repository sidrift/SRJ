using srj.Application.Dtos.Response.Gold;

namespace srj.Application.Interface.Services.Gold;

public interface IGoldJewelryCalculationService
{
    Task<GoldPriceCalculationResponse> CalculateAsync(string sku);
}
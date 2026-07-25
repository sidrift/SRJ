using srj.Application.Dtos.Response.Silver;

namespace srj.Application.Interface.Services.Silver;

public interface ISilverJewelryCalculationService
{
    Task<SilverPriceCalculationResponse> CalculateAsync(string sku);
}
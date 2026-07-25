using srj.Application.Dtos.Request.Silver;
using srj.Domain.Models;

namespace srj.Application.Interface.Services.Silver;

public interface ISilverJewelryItemService
{
    Task<JewelryItem> GetBySkuAsync(string sku);

    Task<JewelryItem> CreateAsync(CreateSilverJewelryRequest request);

    Task<JewelryItem> UpdateAsync(UpdateSilverJewelryRequest request);

    Task DeleteBySkuAsync(string sku);
}
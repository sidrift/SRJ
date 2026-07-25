using srj.Application.Dtos.Request.Gold;
using srj.Domain.Models;

namespace srj.Application.Interface.Services.Gold;

public interface IGoldJewelryItemService
{
    Task<JewelryItem> GetBySkuAsync(string sku);

    Task<JewelryItem> CreateAsync(CreateGoldJewelryRequest request);

    Task<JewelryItem> UpdateAsync(UpdateGoldJewelryRequest request);

    Task DeleteBySkuAsync(string sku);
}
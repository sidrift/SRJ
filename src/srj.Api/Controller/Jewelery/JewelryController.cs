using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Commons;
using srj.Application.Dtos.Request.Jewelery;
using srj.Application.Dtos.Response.Gold;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Services.Gold;
using srj.Application.Interface.Services.Jewelery;
using srj.Application.Interface.Services.Silver;

namespace TheSRJProject.Controller.Jewelery;

[ApiController]
[Route("api/jewelry")]
public class JewelryController : ControllerBase
{
    private readonly IJewelryQueryService _service;

    private readonly IGoldJewelryItemService _goldJeweleryService;

    private readonly ISilverJewelryItemService _silverJeweleryService;

    public JewelryController(IJewelryQueryService service,
        IGoldJewelryItemService goldJeweleryService, ISilverJewelryItemService silverJeweleryService)
    {
        _service = service;
        _goldJeweleryService = goldJeweleryService;
        _silverJeweleryService = silverJeweleryService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<JewelryListResponse>>> GetAll(
        [FromQuery] JewelrySearchRequest request)
    {
        var result = await _service.GetAllAsync(request);

        return Ok(result);
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<object>> GetBySku(string sku)

    {
    if (string.IsNullOrWhiteSpace(sku))
        return BadRequest(new
        {
            Message = "SKU is required."

        });

    try
    {
        if (sku.StartsWith("G-", StringComparison.OrdinalIgnoreCase))
        {
            var gold = await _goldJeweleryService.GetBySkuAsync(sku);


            return new
            {
                metal = gold.Category.Metal.ToString(),

                data = new GoldJewelryDetailsResponse
                {
                    Id = gold.Id,
                    Sku = gold.Sku,

                    Category = gold.Category?.Name ?? string.Empty,

                    WeightInGrams = gold.WeightInGrams,

                    HasStones = gold.HasStones,
                    StoneWeight = gold.StoneWeight,
                    StoneType = gold.StoneType,
                    StonePrice = gold.StonePrice,

                    PurityInPercentage = gold.PurityInPercentage,
                    MakingChargeType = gold.MakingChargeType!.Value,
                    MakingChargePercentage = gold.MakingChargePercentage,
                    MakingChargeWeight = gold.MakingChargeWeight!.Value,
                    HallMarkCharge = gold.HallMarkCharge!.Value,

                    BarcodeImageUrl = gold.BarcodeImageUrl
                }
            };
        }


        if (sku.StartsWith("S-", StringComparison.OrdinalIgnoreCase))
        {
            var silver = await _silverJeweleryService.GetBySkuAsync(sku);


            return new
            {
                metal = silver.Category.Metal.ToString(),

                data = new SilverJewelryDetailsResponse
                {
                    Id = silver.Id,
                    Sku = silver.Sku,

                    Category = silver.Category?.Name ?? string.Empty,

                    WeightInGrams = silver.WeightInGrams,
                    Purity = silver.PurityInPercentage,
                    SilverMakingChargePerGram = silver.SilverMakingChargePerGram ?? 0,

                    BarcodeImageUrl = silver.BarcodeImageUrl
                }
            };
        }


        return BadRequest(new
        {
            Message = "Invalid SKU format. SKU must start with G- or S-."
        });
    }
    catch (KeyNotFoundException)
    {
        return NotFound(new
        {
            Message = "Jewellery item not found."

        });
    }
    }
}
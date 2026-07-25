using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Silver;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Services.Silver;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace TheSRJProject.Controller.Silver;

[ApiController]
[Route("api/jewelry/silver")]
public class SilverJewelryItemController : ControllerBase
{
    private readonly ISilverJewelryItemService _itemService;

    public SilverJewelryItemController(ISilverJewelryItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<SilverJewelryDetailsResponse>> Get(string sku)
    {
        try
        {
            var item = await _itemService.GetBySkuAsync(sku);

            if (item.Category?.Metal != Metal.Silver)
                return BadRequest(new
                {
                    message = $"SKU '{sku}' is not a silver jewellery item."
                });

            return Ok(ToResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<SilverJewelryDetailsResponse>> Create(
        CreateSilverJewelryRequest request)
    {
        try
        {
            var item = await _itemService.CreateAsync(request);

            return Ok(ToResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut]
    public async Task<ActionResult<SilverJewelryDetailsResponse>> Update(
        UpdateSilverJewelryRequest request)
    {
        try
        {
            var item = await _itemService.UpdateAsync(request);

            return Ok(ToResponse(item));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{sku}")]
    public async Task<IActionResult> Delete(string sku)
    {
        try
        {
            await _itemService.DeleteBySkuAsync(sku);

            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    private static SilverJewelryDetailsResponse ToResponse(JewelryItem item)
    {
        return new SilverJewelryDetailsResponse
        {
            Id = item.Id,
            Sku = item.Sku,

            Category = item.Category?.Name ?? string.Empty,

            WeightInGrams = item.WeightInGrams,
            Purity = item.PurityInPercentage,
            SilverMakingChargePerGram = item.SilverMakingChargePerGram ?? 0,

            HasStones = item.HasStones,
            StoneWeight = item.StoneWeight,
            StoneType = item.StoneType,
            StonePrice = item.StonePrice,

            BarcodeImageUrl = item.BarcodeImageUrl
        };
    }
}
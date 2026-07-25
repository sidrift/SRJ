using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Gold;
using srj.Application.Dtos.Response.Gold;
using srj.Application.Interface.Services.Gold;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace TheSRJProject.Controller.Gold;

[ApiController]
[Route("api/jewelry/gold")]
public class GoldJewelryItemController : ControllerBase
{
    private readonly IGoldJewelryItemService _itemService;

    public GoldJewelryItemController(IGoldJewelryItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<GoldJewelryDetailsResponse>> Get(string sku)
    {
        try
        {
            var item = await _itemService.GetBySkuAsync(sku);

            if (item.Category?.Metal != Metal.Gold)
                return BadRequest(new
                {
                    message = $"SKU '{sku}' is not a gold jewellery item."
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
    public async Task<ActionResult<GoldJewelryDetailsResponse>> Create(
        CreateGoldJewelryRequest request)
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
    public async Task<ActionResult<GoldJewelryDetailsResponse>> Update(
        UpdateGoldJewelryRequest request)
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

    private static GoldJewelryDetailsResponse ToResponse(JewelryItem item)
    {
        return new GoldJewelryDetailsResponse
        {
            Id = item.Id,
            Sku = item.Sku,

            Category = item.Category?.Name ?? string.Empty,

            WeightInGrams = item.WeightInGrams,

            HasStones = item.HasStones,
            StoneWeight = item.StoneWeight,
            StoneType = item.StoneType,
            StonePrice = item.StonePrice,

            PurityInPercentage = item.PurityInPercentage,
            MakingChargeType = item.MakingChargeType!.Value,
            MakingChargePercentage = item.MakingChargePercentage,
            MakingChargeWeight = item.MakingChargeWeight!.Value,
            HallMarkCharge = item.HallMarkCharge!.Value,

            BarcodeImageUrl = item.BarcodeImageUrl
        };
    }
}
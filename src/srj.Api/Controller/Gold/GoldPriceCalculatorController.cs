using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Response.Gold;
using srj.Application.Interface.Services.Gold;

namespace TheSRJProject.Controller.Gold;

[ApiController]
[Route("api/calculate-pricing/gold")]
public class GoldPriceCalculatorController : ControllerBase
{
    private readonly IGoldJewelryCalculationService _service;

    public GoldPriceCalculatorController(
        IGoldJewelryCalculationService service)
    {
        _service = service;
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<GoldPriceCalculationResponse>> Calculate(
        string sku)
    {
        try
        {
            var result = await _service.CalculateAsync(sku);

            return Ok(result);
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
}
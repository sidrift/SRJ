using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Services.Silver;

namespace TheSRJProject.Controller.Silver;

[ApiController]
[Route("api/calculate-pricing/silver")]
public class SilverPriceCalculatorController : ControllerBase
{
    private readonly ISilverJewelryCalculationService _service;

    public SilverPriceCalculatorController(
        ISilverJewelryCalculationService service)
    {
        _service = service;
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<SilverPriceCalculationResponse>> Calculate(
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
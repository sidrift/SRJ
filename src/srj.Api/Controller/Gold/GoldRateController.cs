using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Gold;
using srj.Application.Dtos.Response.Gold;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Gold;
using srj.Domain.Models;

namespace TheSRJProject.Controller.Gold;

[ApiController]
[Route("api/gold-rate")]
[Authorize]
public class GoldRateController : ControllerBase
{
    private readonly IIndiaDateTimeService _dateTime;
    private readonly IGoldRateService _service;

    public GoldRateController(
        IGoldRateService service,
        IIndiaDateTimeService dateTime)
    {
        _service = service;
        _dateTime = dateTime;
    }

    [HttpPut]
    public async Task<ActionResult<GoldPriceResponse>> Upsert(CreateGoldPriceRequest request)
    {
        try
        {
            var result = await _service.UpsertAsync(request);

            return Ok(ToResponse(result));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "An unexpected error occurred while processing the gold price."
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<GoldPriceResponse>>> GetAll()
    {
        var prices = await _service.GetAllAsync();

        return Ok(prices.Select(ToResponse));
    }

    [HttpGet("today")]
    public async Task<ActionResult<GoldPriceResponse>> GetToday()
    {
        var price = await _service.GetTodayAsync();

        if (price == null)
            return NotFound();

        return Ok(ToResponse(price));
    }

    [HttpGet("{date}")]
    public async Task<ActionResult<GoldPriceResponse>> GetByDate(DateOnly date)
    {
        var price = await _service.GetByDateAsync(date);

        if (price == null)
            return NotFound();

        return Ok(ToResponse(price));
    }

    private GoldPriceResponse ToResponse(GoldPrice price)
    {
        return new GoldPriceResponse
        {
            PriceDate = price.PriceDate,

            SellPrice24KImp = price.SellPrice24KImp,
            BuyPrice24KImp = price.BuyPrice24KImp,

            SellPrice24KFt = price.SellPrice24KFt,
            BuyPrice24KFt = price.BuyPrice24KFt,

            SellPrice22K = price.SellPrice22K,
            SellPrice20K = price.SellPrice20K,
            SellPrice18K = price.SellPrice18K,

            UpdatedAt = _dateTime.ConvertFromUtc(price.UpdatedAt)
        };
    }
}
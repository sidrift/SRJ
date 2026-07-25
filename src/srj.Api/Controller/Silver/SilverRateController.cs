using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Silver;
using srj.Application.Dtos.Response.Silver;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Silver;
using srj.Domain.Models;

namespace TheSRJProject.Controller.Silver;

[ApiController]
[Route("api/silver-rates")]
public class SilverRateController : ControllerBase
{
    private readonly IIndiaDateTimeService _dateTime;
    private readonly ISilverRateService _service;

    public SilverRateController(
        ISilverRateService service,
        IIndiaDateTimeService dateTime)
    {
        _service = service;
        _dateTime = dateTime;
    }

    [HttpPut]
    public async Task<ActionResult<SilverPriceResponse>> Upsert(CreateSilverPriceRequest request)
    {
        var result = await _service.UpsertAsync(request);

        return Ok(ToResponse(result));
    }

    [HttpGet("today")]
    public async Task<ActionResult<SilverPriceResponse>> GetToday()
    {
        var result = await _service.GetTodayAsync();

        if (result == null)
            return NotFound();

        return Ok(ToResponse(result));
    }

    [HttpGet("{date}")]
    public async Task<ActionResult<SilverPriceResponse>> GetByDate(DateOnly date)
    {
        var result = await _service.GetByDateAsync(date);

        if (result == null)
            return NotFound();

        return Ok(ToResponse(result));
    }

    [HttpGet]
    public async Task<ActionResult<List<SilverPriceResponse>>> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result.Select(ToResponse));
    }

    private SilverPriceResponse ToResponse(SilverPrice price)
    {
        return new SilverPriceResponse
        {
            PriceDate = price.PriceDate,
            SellPriceSilly = price.SellPriceSilly,
            BuyPriceSilly = price.BuyPriceSilly,
            SellPrice99 = price.SellPrice99,
            BuyPrice99 = price.BuyPrice99,
            UpdatedAt = _dateTime.ConvertFromUtc(price.UpdatedAt)
        };
    }
}
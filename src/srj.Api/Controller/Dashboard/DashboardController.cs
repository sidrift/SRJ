using Microsoft.AspNetCore.Mvc;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Dashboard;

namespace TheSRJProject.Controller.Dashboard;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IIndiaDateTimeService _dateTimeService;

    public DashboardController(
        IDashboardService dashboardService,
        IIndiaDateTimeService dateTimeService)
    {
        _dashboardService = dashboardService;
        _dateTimeService = dateTimeService;
    }

    [HttpGet("rates/{date}")]
    public async Task<IActionResult> GetRates(DateOnly date)
    {
        var today = _dateTimeService.Today;
        var earliestAllowed = today.AddDays(-30);

        if (date > today)
        {
            return BadRequest(new
            {
                message = "Future dates are not allowed."
            });
        }

        if (date < earliestAllowed)
        {
            return BadRequest(new
            {
                message = "Rates can only be viewed for the last 30 days."
            });
        }

        var result = await _dashboardService.GetRatesAsync(date);

        if (result == null)
        {
            return Ok(Array.Empty<object>());
        }

        return Ok(result);
    }
}
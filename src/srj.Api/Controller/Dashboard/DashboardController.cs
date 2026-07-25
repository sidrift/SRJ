using Microsoft.AspNetCore.Mvc;
using srj.Application.Interface.Services.Dashboard;

namespace TheSRJProject.Controller.Dashboard;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("rates/today")]
    public async Task<IActionResult> GetTodayRates()
    {
        var result =
            await _dashboardService.GetTodayRatesAsync();


        if (result == null)
            return NotFound(new
            {
                message = "Today's gold and silver rates are not set.",
                priceDate = DateOnly.FromDateTime(DateTime.UtcNow)
            });


        return Ok(result);
    }
}
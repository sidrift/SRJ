using srj.Application.Dtos.Response.Dashboard;

namespace srj.Application.Interface.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardRateResponse?> GetTodayRatesAsync();
}
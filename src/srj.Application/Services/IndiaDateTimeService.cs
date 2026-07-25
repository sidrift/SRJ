using srj.Application.Interface.Services;

namespace srj.Application.Services;

public class IndiaDateTimeService : IIndiaDateTimeService
{
    private static readonly TimeZoneInfo IndiaTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

    public DateOnly Today =>
        DateOnly.FromDateTime(Now);

    public DateTime Now =>
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            IndiaTimeZone);

    public DateTime ConvertFromUtc(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
            IndiaTimeZone);
    }
}
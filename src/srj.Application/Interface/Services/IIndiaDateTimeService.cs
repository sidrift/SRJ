namespace srj.Application.Interface.Services;

public interface IIndiaDateTimeService
{
    DateOnly Today { get; }

    DateTime Now { get; }

    DateTime ConvertFromUtc(DateTime utcDateTime);
}
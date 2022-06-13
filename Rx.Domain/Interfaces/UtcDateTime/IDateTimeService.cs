namespace Rx.Domain.Interfaces.UtcDateTime;

public interface IDateTimeService
{
    DateTime NowUtc { get; }
}
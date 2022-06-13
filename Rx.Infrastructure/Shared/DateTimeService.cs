using Rx.Domain.Interfaces.UtcDateTime;

namespace Rx.Infrastructure.Shared;

public class DateTimeService:IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
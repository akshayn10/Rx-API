namespace Rx.Domain.Interfaces.DbContext;

public interface IDbContext:IDisposable
{
    Microsoft.EntityFrameworkCore.DbContext Instance { get; }
}
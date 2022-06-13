using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.Interfaces.DbContext
{
    public interface IPrimaryDbContext:IDbContext
   
    {
            DbSet<Organization>? Organizations { get; set; }
            Task<int> SaveChangesAsync();
    }
}
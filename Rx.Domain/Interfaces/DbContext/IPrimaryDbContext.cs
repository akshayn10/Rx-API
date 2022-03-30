using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.Entities.Primary;

namespace Rx.Domain.Interfaces.DbContext
{
    public interface IPrimaryDbContext
   
    {
            DbSet<Organization> Organizations { get; set; }
            Task<int> SaveChangesAsync();
        }
}


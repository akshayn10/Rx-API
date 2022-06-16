using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rx.Domain.Entities.Primary;

namespace Rx.Infrastructure.Persistence.Configuration.Primary;

public class PlanConfiguration:IEntityTypeConfiguration<SystemSubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SystemSubscriptionPlan> builder)
    {
        builder.HasData(
            new SystemSubscriptionPlan
            {
                PlanId = new Guid("FF2C0405-FD64-4AA5-B9AA-D3CF68077F91"),
                Name = "Basic",
                Description = "Basic Plan : One User(Owner)",
                Price = 90,
                Duration = 1
            },
            new SystemSubscriptionPlan
            {
                PlanId = new Guid("11B9C80B-86AD-4CD7-B1A7-442D2D30460C"),
                Name = "Pro",
                Description = "Pro Plan : Owner, 3 Admin, 2 Finance Users",
                Price = 140,
                Duration = 1
            },
            new SystemSubscriptionPlan
            {
                PlanId = new Guid("4B329068-F1A3-474A-8C1B-74B182515A5E"),
                Name = "Premium",
                Description = "Premium Plan : Owner, 5 Admin, 10 Finance Users",
                Price = 230,
                Duration = 1
            }
        );
    }
}
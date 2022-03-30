
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rx.Domain.Entities.Tenant;

namespace Rx.Infrastructure.Persistence.Configuration.Tenant
{
    public class OrganizationCustomerConfiguration : IEntityTypeConfiguration<OrganizationCustomer>
    {
        public void Configure(EntityTypeBuilder<OrganizationCustomer> builder)
        {
            builder.HasData(
                new OrganizationCustomer
                {
                    CustomerId = new Guid("BE7E73A2-4DD5-4D2B-BAAA-EEF3A0143593"),
                    Email = "apple@gmail.com",
                    Name = "John Antony",
                    PaymentGatewayId = "1234567"
                }
            );
        }
    }
}

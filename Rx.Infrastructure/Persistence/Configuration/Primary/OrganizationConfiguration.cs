using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rx.Domain.Entities.Primary;

namespace Rx.Infrastructure.Persistence.Configuration.Primary
{
    // public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    // {
    //     public void Configure(EntityTypeBuilder<Organization> builder)
    //     {
    //         builder.HasData(
    //             new Organization
    //             {
    //                 Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
    //                 Name = "Apple",
    //                 TenantId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
    //                 Description = "Mobile Solutions provider",
    //                 LogoURL = "www.apple.com/logo"
    //             }
    //             );
    //     }
    // }
}

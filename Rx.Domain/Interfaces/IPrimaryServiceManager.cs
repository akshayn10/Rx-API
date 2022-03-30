using Rx.Domain.Interfaces.Primary;

namespace Rx.Domain.Interfaces
{
    public interface IPrimaryServiceManager
    {
        IOrganizationService OrganizationService { get; }
    }
}

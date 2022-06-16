using Rx.Domain.Interfaces.Primary;

namespace Rx.Domain.Interfaces
{
    public interface IPrimaryServiceManager
    {
        IOrganizationService OrganizationService { get; }
        IBillService BillService { get; }
        ISystemSubscriptionService SystemSubscriptionService { get; }
        ISystemSubscriptionPlanService SystemSubscriptionPlanService { get; }
        ITransactionService TransactionService { get; }
    }
}

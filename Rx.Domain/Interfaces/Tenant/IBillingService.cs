using Rx.Domain.DTOs.Tenant.Bill;

namespace Rx.Domain.Interfaces.Tenant
{
    public interface IBillingService
    {
        Task GenerateBill (Guid customerId);
        Task<BillVm> GetBillByCustomerId (Guid customerId);
        Task<BillVm> GetBillByBillId (Guid billId);
        Task<IEnumerable<BillVm>> GetBillsByCustomerId (Guid customerId);


    }
}

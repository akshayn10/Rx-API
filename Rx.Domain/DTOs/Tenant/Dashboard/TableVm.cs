namespace Rx.Domain.DTOs.Tenant.Dashboard;

public record TableVm(IEnumerable<TableStats> plan,IEnumerable<TableStats> addOn);
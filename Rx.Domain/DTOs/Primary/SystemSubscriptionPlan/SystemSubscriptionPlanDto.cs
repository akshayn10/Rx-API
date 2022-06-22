namespace Rx.Domain.DTOs.Primary.SystemSubscriptionPlan;

public record SystemSubscriptionPlanDto(string PlanId,
    string Name,
    string Description,
    decimal Price,
    int? Duration
    );
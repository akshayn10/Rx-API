namespace Rx.Domain.DTOs.Tenant.AddOn;

public record AddOnForUpdateDto(Guid AddOnId,string Name,string UnitOfMeasure, Guid ProductId);
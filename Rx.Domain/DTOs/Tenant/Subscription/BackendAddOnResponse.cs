﻿namespace Rx.Domain.DTOs.Tenant.Subscription;

public record BackendAddOnResponse(string EventType,string CustomerId,string SubscriptionId,string AddOnId);
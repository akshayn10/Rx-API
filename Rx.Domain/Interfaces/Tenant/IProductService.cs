namespace Rx.Domain.Interfaces.Tenant
{
    public interface IProductService
    {
        Task<Guid> GetWebhookSecret(Guid productId);
    }
}

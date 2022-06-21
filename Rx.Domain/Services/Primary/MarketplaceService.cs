using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Marketplace;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Primary;

namespace Rx.Domain.Services.Primary;

public class MarketplaceService :IMarketplaceService
{
    private readonly IPrimaryDbContext _primaryDbContext;
    private readonly ILogger<PrimaryServiceManager> _logger;
    private readonly IMapper _mapper;
    private readonly ITenantDbContext _tenantDbContext;

    public MarketplaceService(IPrimaryDbContext primaryDbContext, ILogger<PrimaryServiceManager> logger, IMapper mapper,ITenantDbContext tenantDbContext)
    {
        _primaryDbContext = primaryDbContext;
        _logger = logger;
        _mapper = mapper;
        _tenantDbContext = tenantDbContext;
    }

    public async Task<string> CreateMarketplaceProduct(CreateMarketplaceProductDto createMarketplaceProductDto)
    {
        var organization =await _primaryDbContext.Organizations!.FindAsync(Guid.Parse(createMarketplaceProductDto.OrganizationId!));
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }
        
        var product = new MarketplaceProducts
        {
            Name = createMarketplaceProductDto.Name,
            Description = createMarketplaceProductDto.Description,
            HaveTrial = createMarketplaceProductDto.TrialDays > 0,
            LogoUrl = createMarketplaceProductDto.LogoUrl,
            ProviderName = organization.Name,
            RedirectUrl = createMarketplaceProductDto.RedirectUrl,
            TrialDays = createMarketplaceProductDto.TrialDays,
        };
        await _primaryDbContext.MarketplaceProducts.AddAsync(product);
        await _primaryDbContext.SaveChangesAsync();
        
        // Update tenant product
        var tenantProduct = await _tenantDbContext.Products!.FindAsync(Guid.Parse(createMarketplaceProductDto.ProductId));
        tenantProduct!.IsAddedToMarketplace = true;
        await _tenantDbContext.SaveChangesAsync();
        
        return "Product Added Successfully "+product.ProductId;
    }
}
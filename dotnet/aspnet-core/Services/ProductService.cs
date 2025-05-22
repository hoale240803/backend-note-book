
using Microsoft.EntityFrameworkCore;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IExternalPricingService _pricingService;
    private readonly ILogger<ProductService> _auditLogger;

    public ProductService(ApplicationDbContext context, IExternalPricingService pricingService, ILogger<ProductService> auditLogger)
    {
        _context = context;
        _pricingService = pricingService;
        _auditLogger = auditLogger;
    }

    public async Task<Product> GetProduct(int id)
    {
        var res = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        return res;
    }

    public async Task<Product> UpdateProductPriceAsync(int productId, decimal discount)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) throw new Exception("Product not found");

        var externalPrice = await _pricingService.GetPriceAsync(productId);
        product.Price = externalPrice * (1 - discount);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        _auditLogger.LogInformation($"Updated price for product {productId} to {product.Price}");
        return product;
    }
}

public interface IProductService
{
    Task<Product> GetProduct(int id);
    Task<Product> UpdateProductPriceAsync(int productId, decimal discount);
}
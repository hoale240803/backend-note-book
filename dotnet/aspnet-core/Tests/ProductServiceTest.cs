using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProduct_ReturnsProductById()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Products.Add(new Product { Id = 1, Name = "Laptop1", Price = 999.99m });
        await context.SaveChangesAsync();

        var priceService = new Mock<IExternalPricingService>();
        var loggerService = new Mock<ILogger<ProductService>>();
        var service = new ProductService(context, priceService.Object, loggerService.Object);

        // Act
        var result = await service.GetProduct(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop1", result.Name);
    }
}
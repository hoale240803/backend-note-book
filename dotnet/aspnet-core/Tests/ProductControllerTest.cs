using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductsControllerTests
{
    [Fact]
    public void Get_ReturnsOkResultWithProducts()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB name
        .Options;

        using var context = new ApplicationDbContext(options);
        context.Products.AddRange(
            new Product { Id = 1, Name = "Laptop", Price = 999.99m },
            new Product { Id = 2, Name = "Phone", Price = 499.99m });
        context.SaveChanges();

        var controller = new ProductsController(context);

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
        Assert.Equal(2, returnProducts.Count);
        Assert.Equal("Laptop", returnProducts[0].Name);
    }
}
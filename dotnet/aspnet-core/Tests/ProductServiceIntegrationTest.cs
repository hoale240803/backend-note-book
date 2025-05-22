
using System.Net;
using Moq;
using Moq.Protected;
using Xunit;

public class ProductServiceIntegrationTest
{
    [Fact]
    public async Task GetPriceAsync_ReturnsPrice()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == "https://api.pricing.com/price?productId=1"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(999.99m)
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.pricing.com") // Set BaseAddress
        };
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient("PricingApi")).Returns(httpClient);

        var service = new ExternalPricingService(factoryMock.Object);

        // Act
        var price = await service.GetPriceAsync(1);

        // Assert
        Assert.Equal(999.99m, price);
    }


}
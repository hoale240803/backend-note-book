public class ExternalPricingService : IExternalPricingService
{
    private readonly HttpClient _client;

    public ExternalPricingService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("PricingApi");
    }

    public async Task<decimal> GetPriceAsync(int productId)
    {
        var response = await _client.GetAsync($"price?productId={productId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalPricingException($"Failed to fetch price: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<decimal>();
    }
}

public class ExternalPricingException : Exception
{
    public ExternalPricingException(string message) : base(message) { }
}

public interface IExternalPricingService
{
    Task<decimal> GetPriceAsync(int productId);
}

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LearningDotnetCore.Tests;

public class RateLimiterTests
{
    private readonly HttpClient _client;

    public RateLimiterTests()
    {
        var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
    }

    [Theory]
    [InlineData("FixedWindowPolicy", 5, 10)] // 5 requests allowed in 10 seconds
    [InlineData("SlidingWindowPolicy", 5, 10)] // 5 requests allowed in 10 seconds
    [InlineData("TokenBucketPolicy", 5, 1)] // 5 tokens max, 1 token per second
    [InlineData("ConcurrencyPolicy", 2, 0)] // 2 concurrent requests allowed
    public async Task TestRateLimiter(string policy, int limit, int waitTime)
    {
        // Set the rate-limiting policy dynamically
        _client.DefaultRequestHeaders.Add("RateLimiter-Policy", policy);

        // Send requests up to the limit
        for (int i = 0; i < limit; i++)
        {
            var response = await _client.GetAsync("/weatherforecast");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Send one more request to exceed the limit
        var exceededResponse = await _client.GetAsync("/weatherforecast");
        Assert.Equal(HttpStatusCode.TooManyRequests, exceededResponse.StatusCode);

        // Wait for the rate limiter to reset (if applicable)
        if (waitTime > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            var resetResponse = await _client.GetAsync("/weatherforecast");
            Assert.Equal(HttpStatusCode.OK, resetResponse.StatusCode);
        }
    }
}

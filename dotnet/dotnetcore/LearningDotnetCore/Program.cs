using System.Threading.RateLimiting;

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedWindowPolicy", config =>
    {
        config.PermitLimit = 5; // Allow 5 requests
        config.Window = TimeSpan.FromSeconds(10); // Per 10 seconds
    });

    options.AddSlidingWindowLimiter("SlidingWindowPolicy", config =>
    {
        config.PermitLimit = 5;
        config.Window = TimeSpan.FromSeconds(10);
        config.SegmentsPerWindow = 2; // Divide window into 2 segments
    });

    options.AddTokenBucketLimiter("TokenBucketPolicy", config =>
    {
        config.TokenLimit = 5; // Max 5 tokens
        config.TokensPerPeriod = 1; // Refill 1 token per second
        config.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
    });

    options.AddConcurrencyLimiter("ConcurrencyPolicy", config =>
    {
        config.PermitLimit = 2; // Allow 2 concurrent requests
        config.QueueLimit = 2; // Queue up to 2 requests
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

app.UseRateLimiter();

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
    .ToArray();
    return forecast;
}).RequireRateLimiting("FixedWindowPolicy"); // Change policy as needed
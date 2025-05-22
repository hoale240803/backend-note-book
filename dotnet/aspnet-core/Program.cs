using aspnet_core.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpClient("PricingApi", client =>
{
    client.BaseAddress = new Uri("https://api.pricing.com/");
});
builder.Services.AddScoped<IExternalPricingService, ExternalPricingService>();

var app = builder.Build();


// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//     try
//     {
//         var userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
//         var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//         await ApplicationDbContextSeed.SeedEssensialDataAsync(userManger, roleManager);
//     }
//     catch (Exception ex)
//     {
//         var logger = loggerFactory.CreateLogger<Program>();
//         logger.LogError(ex, "An error occured seeding the db.");
//     }
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

app.UseMaintenanceMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

// app.UseMiddleware<RequestLogginMiddleware>();
// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

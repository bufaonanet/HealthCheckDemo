using BlazorDemo.Data;
using BlazorDemo.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//configure health checks
builder.Services.AddHealthChecks()
    //.AddCheck("Foo Service", () =>
    //{
    //    // do uour checks
    //    // ...
    //    return HealthCheckResult.Degraded("The check of the Foo service did not work well.");
    //}, new[] { "service" })
    //.AddCheck("Bar Service", () =>
    //    HealthCheckResult.Healthy("The check of the Bar service worked."), new[] { "service" })
    .AddCheck<ResponseTimeHealthCheck>("Network speed test", null, new[] { "service" })
    .AddCheck("DataBase", () =>
        HealthCheckResult.Healthy("The check of the database worked."), new[] { "database", "sql" });

builder.Services.AddSingleton<ResponseTimeHealthCheck>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapHealthChecks("/quickhealth", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/services", new HealthCheckOptions
{
    Predicate = reg => reg.Tags.Contains("service"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();

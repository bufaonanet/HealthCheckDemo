using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//configure health checks
//configure health checks
builder.Services.AddHealthChecks()
    .AddCheck("Foo Service", () =>
    {        
        return HealthCheckResult.Degraded("The check of the Foo service did not work well.");
    }, new[] { "service" })
    .AddCheck("Bar Service", () =>
        HealthCheckResult.Healthy("The check of the Bar service worked."), new[] { "service" })
    .AddCheck("DataBase", () =>
        HealthCheckResult.Healthy("The check of the database worked."), new[] { "database", "sql" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

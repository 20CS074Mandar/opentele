using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenTelemetry()
    .WithTracing(traceProviderBuilder =>
    traceProviderBuilder.AddSource(DiagnosticsConfig.activitysource.Name)
    .ConfigureResource(resource=>resource
    .AddService(DiagnosticsConfig.ServiceName))
    .AddAspNetCoreInstrumentation()
    .AddOtlpExporter())
    .WithMetrics(metricsProviderBuilder=>
    metricsProviderBuilder
    .AddMeter(DiagnosticsConfig.meter.Name)
    .AddOtlpExporter());

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

app.Run();


public static class DiagnosticsConfig
{
    public const string ServiceName = "OpenTeleMvc";
    public static ActivitySource activitysource = new ActivitySource(ServiceName);
    public static Meter meter = new Meter(ServiceName);
    public static Counter<long> requestCounter = meter.CreateCounter<long>("app.request_counter");
}
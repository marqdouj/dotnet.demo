using Marqdouj.DotNet.AzureMaps.UI.Services;
using Marqdouj.DotNet.Demo.Shared.AzureMaps;
using Marqdouj.DotNet.Demo.Components;
using Marqdouj.DotNet.Web.Components.Geolocation;
using Marqdouj.DotNet.Web.Components.Services;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

#region Marqdouj.DotNet.Web.Components
builder.Services.AddJSLoggerService();
builder.Services.AddScoped<IGeolocationService, GeolocationService>(); //Also used with Azure Maps demo.
builder.Services.AddScoped<IResizeObserverService, ResizeObserverService>();
#endregion

#region Marqdouj.DotNet.AzureMaps
builder.Services.AddMapConfiguration(builder.Configuration);
builder.Services.AddScoped<IAzureMapsXmlService, AzureMapsXmlService>(); //Only for demo purposes; not required in production.
builder.Services.AddScoped<IMapDataService, MapDataService>(); //Only for demo purposes; simulates getting map data from an API.
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

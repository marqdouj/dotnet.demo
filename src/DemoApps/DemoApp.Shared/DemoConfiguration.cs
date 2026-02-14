using DemoApp.Shared.Models.AzureMaps;
using DemoApp.Shared.Models.BlazorMaps;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Services;
using Marqdouj.DotNet.AzureMaps.UI.Services;
using Marqdouj.DotNet.Web.Components.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoApp.Shared
{
    public enum DemoMode
    {
        Normal,
        Aspire,
    }

    public interface IDemoConfiguration
    {
        DemoMode Mode { get; }
        bool InDevelopment { get; }
    }

    internal class DemoConfiguration(DemoMode mode, bool inDevelopment) : IDemoConfiguration
    {
        public DemoMode Mode { get; } = mode;
        public bool InDevelopment { get; } = inDevelopment;
    }

    public static class DemoConfigurationExtensions
    {
        public static IHostApplicationBuilder AddDemoConfiguration(this IHostApplicationBuilder builder, DemoMode mode)
        {
            var services = builder.Services;

            services.AddSingleton<IDemoConfiguration>(new DemoConfiguration(mode, builder.Environment.IsDevelopment()));

            #region Marqdouj.DotNet.Web.Components
            // These services are obsolete = see 'Modules' demo.
            //services.AddJSLoggerService();
            //services.AddScoped<IGeolocationService, GeolocationService>(); //Also used with Azure Maps demo.
            //services.AddScoped<IResizeObserverService, ResizeObserverService>();
            #endregion

            #region Marqdouj.DotNet.AzureMaps
            builder.Services.AddMapConfiguration(builder.Configuration);
            builder.Services.AddScoped<IAzureMapsXmlService, AzureMapsXmlService>(); //Only for demo purposes; not required in production.
            builder.Services.AddScoped<IMapDataService, MapDataService>(); //Only for demo purposes; simulates getting map data from an API.
            #endregion

            #region Marqdouj.DotNet.AzureMaps.Blazor
            builder.Services.ConfigureMarqdoujAzMapsBlazor(builder.Configuration);
            builder.Services.AddScoped<IAzureMapsUIXmlService, AzureMapsUIXmlService>(); //Only for demo purposes; not required in production.
            builder.Services.AddScoped<IBzMapsDataService, BzMapsDataService>(); //Only for demo purposes; simulates getting map data from an API.
            #endregion

            return builder;
        }
    }
}

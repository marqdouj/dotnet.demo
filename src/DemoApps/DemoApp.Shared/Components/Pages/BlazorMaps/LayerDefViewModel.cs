using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;

namespace DemoApp.Shared.Components.Pages.BlazorMaps
{
    internal class LayerDefViewModel(MapLayerDef layerDef, Position? zoomTo)
    {
        public MapLayerDef LayerDef { get; set { ArgumentNullException.ThrowIfNull(value); field = value; } } = layerDef;
        public bool IsLoaded { get; set; }
        public bool IsNotLoaded => !IsLoaded;
        public string DisplayName => LayerDef.LayerType.DisplayName;
        public Position? ZoomTo { get; set; } = zoomTo;
        public double ZoomLevel { get; set; } = 11;
    }
}
